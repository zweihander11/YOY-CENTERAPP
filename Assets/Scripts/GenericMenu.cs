


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;


public class GenericMenu : MonoBehaviour 
{


	// This script controls the generic Menu prefab behaviour and options

	//References
	public GameObject player; 								// A reference to the player/camera object
	public bool showCentralPanel;								// Is the central panel active?
	public GameObject centralPanel; 							// The center panel of the menu (main)
	public bool showleftPanel;							   	// Is the left panel active?
	//public string centralPaneltitle;							// Title of the center panel
	public GameObject leftPanel;								// The left panel of the menu
	//public string leftPaneltitle;								// Title of the left panel
	public bool showrightPanel;							   	// Is the right panel active?
	public GameObject rightPanel;								// The right panel of the menu
	//public string rightPaneltitle;							// Title of the right panel
	public bool showFollowPanel;							   	// Is the upper follow panel active?
	public GameObject followUpPanel;							// Upper follow panel



	Animator animator;									// Reference to animator component

	public GameObject sphereTemplate;							// Reference to the sphere object where images are loaded
	public Center centerContainer;								// Reference to the center containing all data (.floors.pois.tooltips.categories) 
	[HideInInspector] public CreateMap appController;					// Reference to the controller script that generates JSON file TODO: refactor original with a better name


	public GameObject contentBarPrefab;							// The prefab to be shown in the upper bar. Ex: BreadCrum category prefab. It uses a bend modifier

	[HideInInspector] public Poi currentPoi;


	public List<string> availableCategories;				   		// A collection copy of all categories defined at the center.	


	public float degreesToRotate; 								// The amount of rotation applied when changing the panel view
	private bool cw;									// Is the menu rotating in a clockwise direction?
	private float degrees;									// Internal variable to calculate spinning time

	static GenericMenu instance = null;                        			 	// Set a singleton so this object persists and can be accessed from different scripts

	public List<string> selectCategories= new List<string>();  		    		// List of selected categories
	public List<string> intersectedCategories = new List<string>();
	public List<Tooltip> intersectTooltip = new List<Tooltip>();
	public List<string> catDisplayText;
	private GameObject obj;


	public string tooltipState = null;

	public GameObject categoryPrefab;
	private int displayCategoryCount = 20;	 						// The amount of categories to display in the menu. Fixed to fit menu design
	private List<GameObject> categoryButtons;
	public int currentCatPage;
	public int incremental ; 							// A factor to determine the distance between breadcrum catogeories
	private int inc;
	private List<string> tempIntersected =  new List<string>();

	public static GenericMenu Instance
	{
		get
		{

			if (instance == null)
			{

				instance = (GenericMenu)FindObjectOfType(typeof(GenericMenu));

			}

			return instance;

		}

	}

	public enum MenuState
	{
		HIDDEN,
		ACTIVE,
		INACTIVE,
		TUTORIAL,
		ROTATING,
		PRELOADING,

	}

	public MenuState currentState;

	void Start()
	{

		print ("Start");
		if (Instance != this)
		{
			Destroy (gameObject);
			//Debug.Log ("DestroyedObjectPersist");
		} 
		else
		{

			DontDestroyOnLoad (gameObject);
		}


		appController = sphereTemplate.GetComponent<CreateMap>();		 // Obtain reference to map creator from the main sphere object




		animator = this.GetComponent<Animator>();


		//Update panels
		if (!showCentralPanel) { centralPanel.SetActive(false);} else { centralPanel.SetActive(true);}
		if(!showleftPanel) { leftPanel.SetActive(false);} else { leftPanel.SetActive(true);}
		if(!showrightPanel) { rightPanel.SetActive(false);} else { rightPanel.SetActive(true);}
		if(!showFollowPanel) { followUpPanel.SetActive(false);} else { followUpPanel.SetActive(true);}

		centralPanel.transform.GetChild(0).GetComponent<ControllerMap>().CalltoScript();

	

		SetMenuState (MenuState.PRELOADING); 



	}

	public void SetMenuState(MenuState newState)
	{

		//(1) State exit actions
		switch (currentState) 
		{
		case MenuState.ACTIVE:
			//Debug.Log ("Out of Loading State");
			break;

		case MenuState.ROTATING:
			//Debug.Log ("Out of Rotating State");
			break;
		}
		// (2) Change Current State
		currentState = newState;

		//(3) State enter actions
		switch (currentState) 
		{

		case MenuState.HIDDEN:
			//Debug.Log ("Into Hidden State");
			centralPanel.SetActive (false);
			leftPanel.SetActive (false);
			rightPanel.SetActive (false);
			break;


		case MenuState.ACTIVE:
			//Debug.Log ("Into Active State");
			if (showCentralPanel) { centralPanel.SetActive (true);}
			if (showleftPanel) { leftPanel.SetActive (true);  }
			if (showrightPanel) { rightPanel.SetActive (true); }
			if (showFollowPanel) { followUpPanel.SetActive (true); }


			//StartCoroutine ("ActivateMap");
			break;

		case MenuState.ROTATING:
			//Debug.Log ("Into Rotating State");
			break;

		case MenuState.PRELOADING:
			break;
		}


	}

	void Update()
	{

		switch (currentState)
		{

		case MenuState.ACTIVE:
			ActiveUpdate ();
			break;
		case MenuState.ROTATING:
			RotatingUpdate ();
			break;
		case MenuState.PRELOADING:
			PreloadingUpdate ();
			break;
		}


	}

	public void ActiveUpdate() 
	{



	}
	public void PreloadingUpdate()
	{

		if(availableCategories != null)
		{
			SetMenuState (MenuState.ACTIVE);
		}

	}
	public void RotatingUpdate()
	{

		degrees -= Time.deltaTime;

		if (cw && degreesToRotate > 0) 
		{
			print (degrees);
			this.gameObject.transform.Rotate (new Vector3(0,degrees,0));

		} 
		else if (!cw && degreesToRotate > 0) 
		{
			print (degrees);
			this.gameObject.transform.Rotate (new Vector3(0,-degrees,0));

		}
		if (degrees < 0) 
		{

			SetMenuState (MenuState.ACTIVE);
		}

	}




	public void RotateMenu2Right() 							// angle: The amount of rotation applied in degrees
	{

		//animator.Play ("MenuRotate");
		cw = true;
		degrees = degreesToRotate;
		SetMenuState (MenuState.ROTATING);



	}
	public void RotateMenu2Left() 
	{

		//animator.Play ("MenuRotateRight");
		cw = false;
		degrees = degreesToRotate;
		SetMenuState (MenuState.ROTATING);

		//this.gameObject.transform.Rotate (new Vector3(0,degreesToRotate,0));

	}


	public void UpdateMenu()
	{


		appController = sphereTemplate.GetComponent<CreateMap>();
		availableCategories = appController.categories;
	
		
		SetMenuState (MenuState.ACTIVE);


	}

	public void AddSelectCategories(string nameCategory)
	{
		
		if (!selectCategories.Contains (nameCategory)) 
		{

			selectCategories.Add (nameCategory);
			GameObject catPrefab = Instantiate (contentBarPrefab, followUpPanel.transform,false); 				//Add breadcrum prefab
			catPrefab.transform.GetChild (0).GetComponent<TextMeshPro> ().text = nameCategory;
			catPrefab.transform.Translate (0,0.6f, 0);
			catPrefab.transform.Rotate (0, inc, 0);
			catPrefab.SetActive (true);
			catPrefab.AddComponent<EventTrigger> ();
			EventTrigger.Entry entry = new EventTrigger.Entry ();
			entry.eventID = EventTriggerType.PointerClick;
			entry.callback.AddListener ((data) => DeleteSelectCategories (nameCategory, catPrefab));
			catPrefab.GetComponent<EventTrigger> ().triggers.Add (entry);
			inc = inc + incremental;										 // Multiplier factor to add categories and move them consecutively
			UpdateAddIntersection ();

		}

	}

	public void DeleteSelectCategories(string nameCategory, GameObject bcprefab)
	{

		print ("Deleted: " + nameCategory);
		var itemToRemove = selectCategories.Single(x => x == nameCategory);
		selectCategories.Remove(itemToRemove);
		UpdateDeleteIntersection();     
		Destroy(bcprefab);

	}


	public void UpdateAddIntersection()
	{
		
		var intersected = selectCategories.Intersect (appController.categories);

		foreach (string intcat in intersected) 
		{

			if (!intersectedCategories.Contains (intcat))
			{

				intersectedCategories.Add (intcat);
				print ("Intersected List " + intcat);

			}


			
		}






		UpdateIntersectTooltip();

	
	}


	private void UpdateIntersectTooltip()  // TODO: Check this function as it might not be working properly
	{


		foreach (Tooltip ttip in appController.pieces) 
		{


			foreach (string item in ttip.category) 
			{
				
				if (intersectedCategories.Contains (item))
				{
					
					print ("Intersected Item in tooltip " );
					tempIntersected.Add (item);
					intersectTooltip.Add (ttip);


				}

				/*if (tempIntersected.Equals(intersectedCategories)) 
				{

					intersectTooltip.Add (ttip);
					print ("Iguales: " );

				}
				*/				
			}


			if (rightPanel.activeSelf != null)			 // Initializes browser panel if there is one
			{


				rightPanel.transform.GetComponentInChildren<BrowserPanel> ().InitializePanel ();
			}

		}  



	}


	public void UpdateDeleteIntersection()
	{   


		var intersect = selectCategories.Intersect(availableCategories);
		intersectedCategories.Clear();
		Debug.Log(intersect.Count());

	foreach (string s in intersect)
		{
			Debug.Log("Intersects " + s);
			if (!intersectedCategories.Contains(s))
			{
				intersectedCategories.Add(s);
			}

		}

	foreach (string s in intersectedCategories)
		{
			Debug.Log("Interceptada " + s);
		}

		Debug.Log(intersectTooltip.Count);
		UpdateIntersectTooltip();
	}


	public void PopulateCategories()
	{

		currentCatPage = 1;

		foreach (string category in appController.categories) // Load all category labels
		{
		
			catDisplayText.Add (category);

		}



		if (appController.categories.Count < displayCategoryCount)  /// If there's only 1 page
		{ 
			
			for (int i = 1; i < appController.categories.Count; i++)
			{
		
			//Find text containers and assign text read from center

				leftPanel.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshPro>().text = catDisplayText[i - 1];
				leftPanel.transform.GetChild(i).gameObject.name = catDisplayText[i - 1];
				//print ("added");

		
			}


			for (int e = appController.categories.Count; e <= displayCategoryCount; e++) 
			{
				
			// Hide empty containers

				leftPanel.transform.GetChild(e).gameObject.SetActive (false);

			}
			
		} 
		else // If there's more than one page
		{

			for (int h = 1; h <= displayCategoryCount; h++)
			{
				
				leftPanel.transform.GetChild (h).transform.GetChild (0).GetComponent<TextMeshPro> ().text = catDisplayText[h - 1];

			}
			
		

		}


	

		foreach (Transform obj in leftPanel.transform)
		{

			if (obj.gameObject.tag == "category") 
			{


				obj.gameObject.AddComponent<EventTrigger>();
				EventTrigger.Entry entrybtn = new EventTrigger.Entry ();
				entrybtn.eventID = EventTriggerType.PointerClick;
				entrybtn.callback.AddListener ((data) => AddSelectCategories(obj.gameObject.name));
				obj.gameObject.GetComponent<EventTrigger> ().triggers.Add (entrybtn);



			}

			
		}





	}

	public void LoadPage(bool next) 					// Function called from menu buttons to navigate category pages
	{


		int t = 1;
		if (next)
		{
			
			currentCatPage++;

			for (int i = currentCatPage*10; i < appController.categories.Count; i++)
			{

				//Find text containers and assign text read from center

				leftPanel.transform.GetChild (t).transform.GetChild (0).GetComponent<TextMeshPro> ().text = catDisplayText[i];
				leftPanel.transform.GetChild (t).gameObject.SetActive (false); // Category won't update until the object is reactivated, for some reason :/ 
				print ("Loaded page: " + currentCatPage);
				leftPanel.transform.GetChild (t).gameObject.SetActive (true); //TODO Can be implemented to add animation effect though...
				t++;


				if (i > appController.categories.Count)
				{

					break;

				}
				//TODO: Hide unused containers

			}


		} 
		else 
		{

			currentCatPage--;


			for (int i = currentCatPage*10; i < appController.categories.Count; i++)
			{

				//Find text containers and assign text read from center

				leftPanel.transform.GetChild (t).transform.GetChild (0).GetComponent<TextMeshPro> ().text = catDisplayText[i];
				leftPanel.transform.GetChild (t).gameObject.SetActive (false); // Category won't update until the object is reactivated, for some reason :/ 
				print ("Loaded page: " + currentCatPage);
				leftPanel.transform.GetChild (t).gameObject.SetActive (true); //TODO Can be implemented to add animation effect though...
				t++;


			

			}

		}



	}

}

