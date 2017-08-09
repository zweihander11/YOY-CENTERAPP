using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.VR;
using TMPro;
using System.Linq;
using System.Collections.Specialized;
using System.IO;

public class CreateMap : MonoBehaviour
{


	static CreateMap instance = null;                               //Set a singleton so this object persists and can be accessed from different scripts

	public static CreateMap Instance
	{
		get
		{

			if (instance == null)
			{
				instance = (CreateMap)FindObjectOfType(typeof(CreateMap));
			}
			return instance;

		}
	}

	Loader loader;
	public GvrViewer gvrViewer;
	MediaPlayerCtrl mediaPlayer;
	MediaPlayerCtrl audioMediaPlayer;
	public GameObject sphere3;
	private GameObject Audio;
	private GameObject soundGuide;
	//Fields
	public string serverUrl;
	public GameObject Button;
	public Canvas canvas; 
	public Center centerTemp; 							 //Changed this to publi in order to be accessible by menu
	private Poi poiTemp;
	private Button newButton;
	private GameObject goButton;
	private GameObject buttonParent;
	private GameObject buttonParentMap;
	private int nextId = 0;
	// Prev id is used the GoBack function to 
	private Tooltip tooltipTemp;
	private Tooltip checkTooltip;
	private int countItem;
	private Vector3 posButton;
	private Vector3 rotButton;
	private string nameButton;
	private GetJSON getJSON;
	private ReadJSONFILE readJSON;
	public string type;
	public AudioSource source;
	public WWW www;


	public GameObject videoPlane;                                   // Navigation variables				
	public GameObject imagePlane;
    public Poi currentPoi;                                   //Gameobject is in the current Poi
    public Floor currentFloor;

    public List<string> categories;
	public List<Tooltip> pieces;
	//public List<Dictionary<object, List<string>>> categories;                             // A collection of all categories defined at the center.
	//public Dictionary<object, List<string>> dictCategories;

	// public GameObject hudPanel;
	private GameObject menuMap;
	public GameObject navPanel;
	//public GameObject btnHolder;
	public GameObject audioManager;
	public GameObject menuContainer;
	public GameObject textContainer;
	private float camRotAngle;
	public GameObject showMenuButton;
	//public GameObject mainMenu;
	public GameObject audioPanel;
	public GameObject tooltipAudioPanel;
	public GameObject loadingMessage;
	public GameObject referenceObject;
	public GameObject navPointPrefab;
	private ColorBlock theColor;



	public Texture2D blackTexture;

	private Transform MainCameraLeftTemp;
	private Transform MainCameraRightTemp;
	private GameObject ImagePlaneTempLeft;
	private GameObject ImagePlaneTempRight;
	public GameObject MainCamera;
	public bool btnActive;												 // Is the show menu buton active?

	private AppStateType currentState;                                  //Store the current state in a variable
	private int contImage;
	private string nextFloorID;
	private string prevFloorID;
	private string prevPoiID;
	private string nextPoiId;
	private bool play= false;
	private bool stop = false;   



	public int posFloor;
	public IGvrPointer pointer;




	private Texture2D tex;  					// Declaration to hold temporary downloaded textures
	public List<string> preloaded = new List<string>();		// Texture ids saved from preloading function
	public List<Texture2D> savedImages = new List<Texture2D>();	// A list where we store downloaded 360 image textures




	public enum AppStateType                                        //Define states the app could be at any given time
	{


		STATE_APP_IDLE,
		STATE_APP_IDLEBTN,   // Same as Idle state used to control appearance of menu button.
		STATE_APP_LOADING,
		STATE_APP_IMAGE,
		STATE_APP_VIDEO,
		STATE_APP_TEXT,
		STATE_APP_360,
		STATE_APP_MENU,
		STATE_APP_TUTORIAL,
		// TODO put this in image
		//STATE_APP_NAVIGATION,  // TODO put this in image

	}



	//private AppStateType nextState;                                     // Store the next state to set after loading


	IEnumerator Start()
	{
		
		
		loader = GameObject.Find ("Loader").GetComponent<Loader>(); // This script now requires a loader object to be present from prior scene
		gvrViewer.VRModeEnabled = loader.vrMode;
		MainCamera = GameObject.Find ("Main Camera");
		mediaPlayer = videoPlane.transform.GetChild(0).GetComponent<MediaPlayerCtrl>();
		audioMediaPlayer = audioManager.GetComponent<MediaPlayerCtrl>();




		//FUNCION FIX PROTOTIPE   
		readJSON = GameObject.Find("sphere3").GetComponent<ReadJSONFILE>();
		//navPanel = GameObject.Find("GameObject");
		CreateGameObjectButton();   // Called this function at first to create navpanel object needed

		sphere3 = gameObject;
		//centerTemp = readJSON.center;                                    //Rescatamos el objeto scene que contiene toda la informacion del JSON   
		//centerTemp =  this.GetComponent<ReadJSONFILE>().center;
		//Debug.Log(centerTemp.firstPoiID);          
		//InitScene(centerTemp.firstPoiID, centerTemp.firstFloorID);    

		while (readJSON.end == false)                                       //Esperamos que termine de leer el JSON
		{

			yield return new WaitForSeconds(0.2f);

		}

		if (readJSON.end == true)
		{
			sphere3 = gameObject;
			//centerTemp = readJSON.center;                                    //Rescatamos el objeto scene que contiene toda la informacion del JSON   
			centerTemp =  this.GetComponent<ReadJSONFILE>().center;



			//Add Total poi ids for this floor into a list For preloading purpose we check wether the Poi has been previously downloaded
			foreach (Floor f in centerTemp.floors) 
			{
				foreach (Poi p in f.pois)
				{


					preloaded.Add (p._id);	



				}
			}




			//Debug.Log (centerTemp.floors[0].pois[0]._id);
			//Debug.Log("poiID: "+centerTemp.firstPoiID+ "floorID: "+centerTemp.firstFloorID);
			InitScene(centerTemp.firstPoiID, centerTemp.firstFloorID);                            //LLamamos a aplicar la primera escena por primera vez, rescantando el firsphotoId             
			//BrowseTooltip(poiTemp);                                       //Buscamos los tootltip que tiene la imagen


		}

        
		//  SetAppState(AppStateType.STATE_APP_IDLE);   						// Changed this intruction to the DownloadImage function. Once we load, we change to idle state and await user input

		menuContainer.GetComponent<GenericMenu>().UpdateMenu();
		//preloaded.Add (null);
		//savedImages.Add (null);

		//SetAppState(AppStateType.STATE_APP_LOADING); 						//We call this funtion to set the LOADING state as initial state when the app starts




	}







	public void SetAppState(AppStateType newState)
	{
		float largePhoto = 0;
		// (1) State Exit Actions:
		switch (currentState)
		{

		case AppStateType.STATE_APP_LOADING:
			// Debug.Log("Out of Loading State");
			//SetAppState (nextState);
			loadingMessage.SetActive(false);
			break;
		case AppStateType.STATE_APP_IMAGE:
			// Debug.Log("Out of Image State");    
			menuContainer.SetActive(false);
			MainCameraLeftTemp = GameObject.Find("Main Camera").transform.GetChild(1);
			MainCameraRightTemp = GameObject.Find("Main Camera").transform.GetChild(2);
			MainCameraLeftTemp.GetComponent<GvrEye>().toggleCullingMask = LayerMask.GetMask("Nothing");
			MainCameraRightTemp.GetComponent<GvrEye>().toggleCullingMask = LayerMask.GetMask("Nothing");
			ImagePlaneTempLeft = imagePlane.transform.GetChild(0).gameObject;
			ImagePlaneTempRight = imagePlane.transform.GetChild(1).gameObject;


			ImagePlaneTempLeft.layer = 0;
			ImagePlaneTempRight.layer = 0;
			//MainCamera = GameObject.Find("Main Camera").transform.gameObject;
			MainCamera.GetComponent<StereoController>().UpdateStereoValues();
			audioMediaPlayer.Stop();
			//soundGuide.SetActive(false);   DESCOMENTAR LUEGO DE PRUEBA

			break;
		case AppStateType.STATE_APP_VIDEO:
			//Debug.Log("Out of Loading State");
			videoPlane.transform.GetChild(0).GetComponent<MediaPlayerCtrl>().Stop();
			videoPlane.transform.GetChild(0).GetComponent<MediaPlayerCtrl>().UnLoad();
			videoPlane.SetActive(false);
			menuContainer.SetActive(false);
			break;
		case AppStateType.STATE_APP_TEXT:
			soundGuide.SetActive(false);
			//Debug.Log("Out Loading State");
			break;
		case AppStateType.STATE_APP_IDLE:
			//Debug.Log("Out of Idle State");
			//navPanel.SetActive (false);
			break;
		case AppStateType.STATE_APP_IDLEBTN:
			//Debug.Log("Out of Idle btn State");
			//ShowMenuButton ();

			break;

		}

		// (2) Change Current State
		currentState = newState;


		// (3) State Enter Actions:
		switch (currentState)
		{

		case AppStateType.STATE_APP_IDLE:                                    //Show navigation panel with navigation buttons
			//sphere3.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
			// Debug.Log("Into Idle State");

			navPanel.SetActive(true);
			menuContainer.SetActive(false);
			imagePlane.SetActive(false);

			//  btnHolder.SetActive(true);
			break;



		case AppStateType.STATE_APP_IDLEBTN:
			ShowMenuButton ();
			//StartCoroutine ("Hide");
			break;

		case AppStateType.STATE_APP_IMAGE:                                  //Load image here 
			Debug.Log("Into Image State");

		//	sphere3.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1);
			imagePlane.SetActive(true);
			navPanel.SetActive(false);

			imagePlane.transform.rotation = new Quaternion(0, 0, 0, 0);    //Rotate the image plane so it's located in front of the camera. Reset rotation first
			camRotAngle = GameObject.Find("Main Camera").transform.rotation.eulerAngles.y;
			imagePlane.transform.Rotate(new Vector3(0, camRotAngle, 0), Space.Self);

			//imagePlane.transform.GetChild(1).transform.localPosition = new Vector3(imagePlane.transform.GetChild(0).position.x , imagePlane.transform.GetChild(0).position.y, imagePlane.transform.GetChild(0).position.z);
			largePhoto = imagePlane.transform.localScale.x;

			// imagePlane.transform.GetChild(1).transform.localPosition = new Vector3(imagePlane.transform.GetChild(0).localScale.x, imagePlane.transform.GetChild(0).localScale.y, imagePlane.transform.GetChild(0).position.z);
			//largePhoto = imagePlane.transform.localScale.x;

			// imagePlane.transform.GetChild(1).transform.GetChild(0).rotate

			menuContainer.SetActive(false);
			tooltipAudioPanel.SetActive(true);
			loadingMessage.SetActive(false);

			break;
		case AppStateType.STATE_APP_LOADING:
			//sphere3.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1);
			menuContainer.transform.rotation = new Quaternion(0, 0, 0, 0);    //Rotate the image plane so it's located in front of the camera. Reset rotation first
			camRotAngle = GameObject.Find("Main Camera").transform.rotation.eulerAngles.y;
			menuContainer.transform.Rotate(new Vector3(0, camRotAngle, 0), Space.Self);
			menuContainer.SetActive(true);
			videoPlane.SetActive(false);
			navPanel.SetActive(false);
			menuContainer.SetActive(false);
			audioPanel.SetActive(false);
			tooltipAudioPanel.SetActive(false);
			//loadingMessage.GetComponent<Text>().text = "Cargando...";
			loadingMessage.SetActive(true);

			imagePlane.transform.rotation = new Quaternion(0, 0, 0, 0);    //Rotate the image plane so it's located in front of the camera. Reset rotation first
			camRotAngle = GameObject.Find("Main Camera").transform.rotation.eulerAngles.y;
			imagePlane.transform.Rotate(new Vector3(0, camRotAngle, 0), Space.Self);
			break;   



		case AppStateType.STATE_APP_TEXT:
			//sphere3.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1);
			Debug.Log("Into Loading State");
			menuContainer.SetActive(true);
			textContainer.SetActive(true);
			textContainer.transform.rotation = new Quaternion(0, 0, 0, 0);
			camRotAngle = GameObject.Find("Main Camera").transform.rotation.eulerAngles.y;
			textContainer.transform.Rotate(new Vector3(0, camRotAngle, 0));
			menuContainer.SetActive(false);
			tooltipAudioPanel.SetActive(true);
			loadingMessage.SetActive(false);
			break;

		case AppStateType.STATE_APP_VIDEO:
			//sphere3.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1);
			Debug.Log("Into Video State");
			videoPlane.SetActive(true);
			//videoPlane.transform.GetChild(0).GetComponent<MediaPlayerCtrl>().m_strFileName = 
			navPanel.SetActive(false);
			videoPlane.transform.rotation = new Quaternion(0, 0, 0, 0);
			camRotAngle = GameObject.Find("Main Camera").transform.rotation.eulerAngles.y;               
			videoPlane.transform.Rotate(new Vector3(0, camRotAngle, 0));
			menuContainer.SetActive(true);
			menuContainer.transform.rotation = new Quaternion(0, 0, 0, 0);
			menuContainer.transform.Rotate(new Vector3(0, camRotAngle, 0));
			menuContainer.SetActive(false);
			tooltipAudioPanel.SetActive(true);
			loadingMessage.SetActive(false);
			break;

			case AppStateType.STATE_APP_MENU:                                    //Show navigation panel with navigation buttons
			//sphere3.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1); // Obscure sphere 
			//Debug.Log ("Into Menu State");
			navPanel.SetActive (false);
			//btnHolder.SetActive(false);
			menuContainer.SetActive (true);
			// audioPanel.SetActive(true);
			tooltipAudioPanel.SetActive (false);
			loadingMessage.SetActive (false);
			menuContainer.transform.rotation = new Quaternion (0, 0, 0, 0);
			camRotAngle = MainCamera.transform.rotation.eulerAngles.y - 90;
			print (camRotAngle);
			menuContainer.transform.Rotate (new Vector3 (0, camRotAngle, 0));
			//print (camRotAngle);
			break;

		case AppStateType.STATE_APP_360:
			//sphere3.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1);
			Debug.Log("Into 360 State");
			navPanel.SetActive(false);
			menuContainer.SetActive(false);
			audioPanel.SetActive(false);
			tooltipAudioPanel.SetActive(false);
			loadingMessage.SetActive(false);
			break;


		}


	}


	public void InitScene(string poiID,string floorID)
	{

		Floor itemFloor = centerTemp.floors.Find(x => x._id == floorID);
		Poi itemPoi = itemFloor.pois.Find(x => x._id == poiID);
		currentFloor = itemFloor;
		currentPoi = itemPoi;
		BrowseTooltip(itemPoi);

	
			//load locally
			Resources.UnloadUnusedAssets ();
			sphere3.GetComponent<Renderer>().material.mainTexture = Resources.Load( poiID ) as Texture;
			SetAppState(AppStateType.STATE_APP_IDLE);
			print ("Map Init from disk");

		if (sphere3.GetComponent<Renderer>().material.mainTexture == null)
		{

			SetAppState(AppStateType.STATE_APP_LOADING);
			StartCoroutine(DownloadImage(itemPoi.uri, itemPoi._id));
			StartCoroutine(Reload360Image(itemPoi));
			preloaded.Add (itemPoi._id);
	
		}

		audioMediaPlayer.Stop();

		if (itemPoi.soundGuide != null)
		{
			audioMediaPlayer.Load(itemPoi.soundGuide);
			audioMediaPlayer.Play();
		}

	}



	public void ApplyScene(string floorApplyScene, string tooltipApplyScene)                          //Metodo para aplicar y cargar cada imagen
	{
		

		var itemFloor = centerTemp.floors.Find (x => x._id == floorApplyScene);
		var itemPoi = itemFloor.pois.Find (x => x._id == tooltipApplyScene);



		Resources.UnloadUnusedAssets();
		//load locally
		tex = sphere3.GetComponent<Renderer> ().material.mainTexture as Texture2D;
		sphere3.GetComponent<Renderer> ().material.mainTexture = Resources.Load (itemPoi._id) as Texture;

		poiTemp = itemPoi;
		BrowseTooltip (itemPoi);
		SetAppState (AppStateType.STATE_APP_IDLE);
		print ("Map Loaded from disk");

		if (sphere3.GetComponent<Renderer> ().material.mainTexture == null) 
		{
			sphere3.GetComponent<Renderer> ().material.mainTexture = tex;
			SetAppState (AppStateType.STATE_APP_LOADING);
			//Debug.Log("currentFloor: "+currentFloor+ "y floorApplyScene: "+floorApplyScene);

			if (currentFloor._id.Equals (floorApplyScene)) 
			{       
				
				poiTemp = itemPoi;
				BrowseTooltip (itemPoi);

				StartCoroutine (DownloadImage (itemPoi.uri, itemPoi._id));
				StartCoroutine (Reload360Image (itemPoi));
			
				audioMediaPlayer.Stop ();
				if (itemPoi.soundGuide != null) 
				{
					audioMediaPlayer.Load (itemPoi.soundGuide);
					audioMediaPlayer.Play ();
				}
			} else {
				
				Debug.Log ("Cambio de piso a: " + itemFloor.floorNumber);
				Destroy (buttonParentMap);
				//CreateMap(itemFloor);          
				//CreateButtonMap();
				currentFloor = itemFloor;

			}

			//preloaded.Add (itemPoi._id);
		


		}
	}

	private void ReadRotationScene(float rotationOffset)
	{

	sphere3.GetComponent<Renderer>().material.mainTextureOffset =  new Vector2( (rotationOffset) /360,0);
	/*
        Vector3 v3 = new Vector3(-90, rotationOffset, 0);
        Quaternion rotationSphere = Quaternion.Euler(new Vector3(270, Mathf.Abs(90 - rotationOffset), 0));
        sphere3.transform.localRotation. ;
        */
	//sphere3.transform.localRotation = rotationSphere;

	//sphere3.transform.rotation = rotationSphere;
	//Rotate(0,0, rotationOffset)  ;
	Debug.Log("offset sphere: "+ rotationOffset);
	Debug.Log("rotanto  sphere global: " + sphere3.transform.rotation.eulerAngles);
	Debug.Log("rotanto  sphere local: " + sphere3.transform.localRotation.eulerAngles);
	//GameObject.Find("Player").transform.rotation = Quaternion.Euler(0, camRotAngle, 0);
	}

	private void StopAudio()
	{
		StopCoroutine("PlayAudio");
		source.Stop();
	}


	private void CreateGameObjectButton()                                   //Metodo que crea un nuevo objeto y lo establece como child de Panel (para los botones)
	{        
		buttonParent = new GameObject("GameObjectButton");       
		//buttonParent.transform.SetParent(canvas.transform , false); // Commented for buttons as gameobjects and not canvas
		navPanel = buttonParent;


	}

   


	//TODO Fix this function to be more elegant
	public void ChangeScene(string floorChangeScene,string tooltipChangeScene,Poi nextPoi)                            //Metodo que es llamado cuando se hace click en algun boton
	{        

		SetAppState(AppStateType.STATE_APP_LOADING);


		 
		foreach (Transform tr in buttonParent.transform)   		//Destroy children to clear memory
		{

			Destroy (tr.gameObject);

		}



			
			Debug.Log ("Siguiente poi a cargar es: " + tooltipChangeScene + " Poi next: " + nextPoi._id);
			currentPoi = nextPoi;


      		
		// Destroy(buttonParent);    Obsoleto, da problemas al perderse la referencia  	




		ApplyScene(floorChangeScene, tooltipChangeScene);    
		ReadRotationScene (nextPoi.rotationOffset);
		menuContainer.GetComponentInChildren<ControllerMap> ().SwitchfromNav (nextPoi._id);


	}


	public void CreateMaps(Floor floor) 											                                        //Changed to public to be accessible by menu
	{	
        	
		StartCoroutine(DownloadImage(floor.uriMap,"MAP"));
        
		/* 
        Sprite image =Resources.Load("Mapas/MAPA_"+floor) as Sprite;
        Debug.Log(image);       
        //sphere3.GetComponent<Renderer>().material.mainTexture = www.texture as Texture;
        menuContainer.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite= image;
        */
		//buttonParentMap = new GameObject("GameObjectButton");
		//buttonParentMap.transform.SetParent(menuContainer.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform, false);
		//buttonParentMap.transform.SetParent(menuMap.transform, false);
        
		//Create floor buttons from center data 
		//TODO: program creation when there's +5 buttons
		/*
		foreach (Floor fl in centerTemp.floors) 
		{
			GameObject floorbtn = Instantiate (mainMenu.GetComponent<GenericMenu> ().centralPanel.GetComponent<GenericPanel> ().buttonPrefab, mainMenu.GetComponent<GenericMenu>().centralPanel.GetComponent<GenericPanel>().buttonPlacer.gameObject.transform, false);
			floorbtn.transform.Translate (0, fl.floorNumber*-1 ,0);
			floorbtn.transform.GetChild (0).GetComponent<TextMeshPro>().text = (fl.floorNumber + 1).ToString();
			floorbtn.SetActive (true); 								 // Activate the button since the prefab must be inactive in order to exist in the scene prior this step
		}


		//mainMenu.GetComponent<GenericMenu>().centralPanel.
		foreach (Floor f in centerTemp.floors) // Create map pois : modified function made by Alejandro in order to work with GameObjects instead of buttons
		{
			if (floor.floorNumber ==f.floorNumber )
			{
				int i = 0;
				foreach (Poi p in f.pois)
				{
					//Debug.Log("position "+p.position[0]+" "+p.position[1]);

					GameObject point = Instantiate (mainMenu.GetComponent<GenericMenu> ().centralPanel.GetComponent<GenericPanel> ().pointPrefab, mainMenu.GetComponent<GenericMenu> ().centralPanel.GetComponent<GenericPanel> ().map.gameObject.transform.GetChild (0).transform, false); // .map.gameObject.transform
					point.transform.Translate (new Vector3 (-5.7f, p.position [0] * -0.012f, p.position [1] * -0.012f), Space.Self);
					point.transform.localScale = new Vector3(0.2f, 0.2f,0.2f);
					point.AddComponent<EventTrigger>();
					EventTrigger.Entry entry = new EventTrigger.Entry ();
					entry.eventID = EventTriggerType.PointerClick;
					entry.callback.AddListener((data) => ChangeScene(f._id, p._id,p));
					point.GetComponent<EventTrigger>().triggers.Add (entry);
					point.transform.GetChild(0).GetComponent<TextMeshPro>().text =  (i+1).ToString();
					point.name = p._id;
					i += 1;                                                                 // Increses the visual number

					//Previous function designed to work with canvas and buttons: 
					//  GameObject goButton = Instantiate(Button);
					//goButton.transform.SetParent(buttonParentMap.transform, false);
					// Button newButton = goButton.GetComponent<Button>();
                    

					/*
					newButton.GetComponentInChildren<Text>().text = "";
                    newButton.name = p._id;
					newButton.onClick.AddListener(() => ChangeScene(f._id, p._id, p));
                    goButton.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

                    
					//newButton.transform.Translate(new Vector3(float.Parse(p.mapPos[0].ToString()), float.Parse(p.mapPos[1].ToString()), 0), Space.Self);

				}
			}    
		}*/
	}

	public void LoadFloorMap(GameObject btnId)  			// This function is called from the level buttons at the menu to show a different floor in the map
	{
        //btnId.GetComponent<TextMeshPro> ().text;
		StartCoroutine(DownloadImage(centerTemp.floors[(int.Parse(btnId.GetComponent<TextMeshPro>().text))-1].uriMap ,"MAP"));        
	}



	public IEnumerator DownloadImage(string url, string type)                   //Metodo para descargar algo de internet. Imagenes, texto etc... 
	{


		string tempUrl = serverUrl + url;
		Debug.Log("Se descargara url:" + tempUrl);
		WWW www = new WWW(tempUrl);
		yield return www;

		if (!string.IsNullOrEmpty(www.error))
		{
		SetAppState(AppStateType.STATE_APP_LOADING);
		loadingMessage.GetComponent<Text>().text = "Compruebe su conexión a internet";
		Debug.Log("Check internet conection " + www.error);
		
		while (!string.IsNullOrEmpty(www.error))
		{

			www = new WWW(url);
			yield return www;
		}

	}

	Texture2D tex;
	tex = new Texture2D (www.texture.width, www.texture.height, TextureFormat.ETC2_RGB, false);

	www.LoadImageIntoTexture(tex);


	//AspectRatioFitter.AspectMode.FitInParent;
	switch (type)
	{
	case "image":
		Debug.Log("Case Image");
		//AspectRatio(www.texture.width,www.texture.height, imagePlane);
			imagePlane.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = www.texture;
		SetAppState(AppStateType.STATE_APP_IMAGE);
		break;
	case "PANELIMAGE":
		imagePlane.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = tex;
		SetAppState(AppStateType.STATE_APP_IMAGE);
		Debug.Log("Case Panel Image");
		break;
	case "VIDEO":
		Debug.Log("Case Video");
		videoPlane.transform.GetChild(0).GetComponent<MediaPlayerCtrl>().m_strFileName = www.url;
		break;
	case "taudio":
		Debug.Log("Case Audio");
		break;
	case "GATE":

		sphere3.GetComponent<Renderer>().material.mainTexture = tex;
		//  Destroy(www);
		//SetAppState(AppStateType.STATE_APP_IDLE);
		break;
	case "TEXTBLOCK":
		Debug.Log("Case TextBlock");
		//This line is never used. Code never makes it here.
		//SetAppState (AppStateType.STATE_APP_TEXT);
		break;
	case "360":
		Debug.Log("Case 360");
		sphere3.GetComponent<Renderer>().material.mainTexture = tex;
		SetAppState(AppStateType.STATE_APP_360);
		//LoadImageStrip (url);
		break;
	case "imagestereoleft":
		//AspectRatio(www.texture.width, www.texture.height, imagePlane);
		Debug.Log("Case Image Stereo Left");
		imagePlane.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = tex;
		Debug.Log("Es izquierdo " + www.url);
		break;

	case "imagestereoright":
		Debug.Log("Case Image Stereo Right");
		imagePlane.transform.GetChild(1).GetComponent<Renderer>().material.mainTexture = tex;
		SetAppState(AppStateType.STATE_APP_IMAGE);
		MainCameraRightTemp.gameObject.SetActive(true);
		Debug.Log("Es derecho " + www.url);
		break;
		case "MAP":
		//  AspectRatio(www.texture.width, www.texture.height, menuContainer.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject);
		//  Debug.Log("Case map");
		// menuContainer.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = www.texture;                  
		// Find the Game Object to Apply map texture on:
		/*	if (menuMap != null)
		{
			menuMap = menuContainer.transform.GetChild (0).transform.GetChild (0).gameObject;

			}*/
			//print ("Map change");
		//menuMap.GetComponent<Renderer>().material.mainTexture = null;
//		menuMap.GetComponent<Renderer>().material.mainTexture = tex; 
		break;

	default:
		//Debug.Log("Default case");
		sphere3.GetComponent<Renderer>().material.mainTexture = tex;
		SetAppState(AppStateType.STATE_APP_IDLE);                                          //We only apply the change of state once the image has loaded
		Resources.UnloadUnusedAssets ();

		//ReadRotationScene(poiTemp.rotationOffset);                                         //Se obtiene la rotacion de la camara definida por el JSON
		break;
	}


	}





	public void BrowseTooltip(Poi poiTemp)                              //Metodo para buscar todos los tooltips que contiene la escena y asi ser creados 
	{       

		if (navPanel == null)
		{
			
			CreateGameObjectButton ();     
		
		}

		if (poiTemp.soundGuide != "")
		{
			CreateButtonMenuAudio(null, poiTemp, "scaudioplay");
			//CreateButton(null, poiBrowseTooltipTemp, "scaudiostop");
		}

		//print ("TTps in scene: "+ poiTemp.tooltips.Count);

        foreach (Tooltip tooltipTemp in poiTemp.tooltips)
        {
            if ("GATE" == tooltipTemp.type && tooltipTemp.linkedPoiID != "")
            {
                //nameButton = photoTemp.tooltips[i].linkedPhotoId;
                CreateButton(tooltipTemp, poiTemp, tooltipTemp.type);
            }
            if ("PANO" == tooltipTemp.type && tooltipTemp.attributionUri != null)
            {
                //nameButton = tooltipTemp.attributionUri;
                CreateButton(tooltipTemp, poiTemp, tooltipTemp.type);
            }

            if ("VIDEO" == tooltipTemp.type && tooltipTemp.attributionUri != null)
            {
                //nameButton = tooltipTemp.attributionUri;
                CreateButton(tooltipTemp, poiTemp, tooltipTemp.type);
            }

            if ("PANELIMAGE" == tooltipTemp.type && tooltipTemp.text != null)
            {
                //nameButton = tooltipTemp.attributionUri;
                CreateButton(tooltipTemp, poiTemp, tooltipTemp.type);
            }
            if ("TEXTBLOCK" == tooltipTemp.type && tooltipTemp.text != null)
            {
                //nameButton = tooltipTemp.text;
                CreateButton(tooltipTemp, poiTemp, tooltipTemp.type);
            }
          
            if (tooltipTemp.soundGuide != "")
            {
                CreateButtonAudio(tooltipTemp, poiTemp, "tooltipAudio");
            }

            if ("STEREOSCOPIC" == tooltipTemp.type && tooltipTemp.sourceUriRight != null)
            {
                CreateButton(tooltipTemp, poiTemp, tooltipTemp.type);
            }

        }
        
	}



	private void CreateButtonMenuAudio(Tooltip tooltipTemp,Poi poiButtonMenuAudioTemp, string type) {
		GameObject goButtonPlay = Instantiate(Button);
		GameObject goButtonStop = Instantiate(Button);
		goButtonPlay.transform.SetParent(buttonParent.transform, false);
		goButtonStop.transform.SetParent(buttonParent.transform, false);

		goButtonPlay.transform.localScale = new Vector3(1, 1, 1);
		goButtonStop.transform.localScale = new Vector3(1, 1, 1);
		Button newButtonPlay = goButtonPlay.GetComponent<Button>();
		Button newButtonStop = goButtonStop.GetComponent<Button>();      
		newButtonPlay.transform.SetParent(audioPanel.transform, false);
		//newButtonPlay.transform.SetParent(audioPanel.transform);
		newButtonPlay.name = "play";
		newButtonPlay.onClick.AddListener(() => ManageAudio("play", poiButtonMenuAudioTemp.soundGuide));    
		newButtonStop.transform.SetParent(audioPanel.transform, false);
		newButtonStop.name = "stop";
		newButtonStop.onClick.AddListener(() => audioMediaPlayer.Stop());



	} 


	private void CreateButtonAudio(Tooltip tooltipTemp,Poi poiButtonAudioTemp, string type)
	{
		GameObject goButtonPlay = Instantiate(Button);
		GameObject goButtonStop = Instantiate(Button);
		goButtonPlay.transform.SetParent(buttonParent.transform, false);
		goButtonStop.transform.SetParent(buttonParent.transform, false);
		goButtonPlay.transform.localScale = new Vector3(1, 1, 1);
		goButtonStop.transform.localScale = new Vector3(1, 1, 1);
		Button newButtonPlay = goButtonPlay.GetComponent<Button>();
		Button newButtonStop = goButtonStop.GetComponent<Button>();
		Audio = new GameObject();
		Audio.name = tooltipTemp._id;
		Audio.transform.position = new Vector3(0, 0, 0);
		Audio.SetActive(false);
		Audio.transform.SetParent(tooltipAudioPanel.transform, false);
		Audio.AddComponent<HorizontalLayoutGroup>();        



		newButtonPlay.transform.SetParent(Audio.transform, false);
		newButtonPlay.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);  
		newButtonPlay.GetComponent<Image>().SetNativeSize();
		newButtonPlay.name = "play";

		newButtonPlay.onClick.AddListener(() => ManageAudio("play", tooltipTemp.soundGuide));
		//newButton.GetComponent<Image>().color = Color.clear;
		newButtonStop.transform.SetParent(Audio.transform, false);
		newButtonStop.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
		newButtonStop.name = "stop";
		newButtonStop.GetComponent<Image>().SetNativeSize();
		newButtonStop.onClick.AddListener(() => audioMediaPlayer.Stop());




		newButtonPlay.transform.Translate(new Vector3(0, 0, 0), Space.Self);
		newButtonStop.transform.Translate(new Vector3(0, 0, 0), Space.Self);
		//newButtonPlay.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
		//newButtonStop.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);       
		//newButton.GetComponent<Image>().color = Color.clear;

	}



	public void CreateButton(Tooltip tooltipTemp, Poi poiButtonTemp, string type)     //Metodo para crear botones  
	{
        float x1;
        float y1;
        float z1;
		/*
        GameObject goButton = Instantiate(Button);
        goButton.transform.SetParent(buttonParent.transform, false);
        goButton.transform.localScale = new Vector3(1, 1, 1);
        Button newButton = goButton.GetComponent<Button>();
        newButton.transform.Rotate(new Vector3(0, float.Parse(tooltipTemp.position[0].ToString()), 0), Space.Self);     
        newButton.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);               
        newButton.transform.Translate(new Vector3(0, 0, 3.5f), Space.Self);
        newButton.name = tooltipTemp.type;     
		*/

		GameObject navPoint = Instantiate (navPointPrefab); // .map.gameObject.transform
		navPoint.transform.SetParent(buttonParent.transform, false);
		navPoint.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
		

      


        navPoint.AddComponent<EventTrigger>();
		EventTrigger.Entry entry = new EventTrigger.Entry ();
		entry.eventID = EventTriggerType.PointerClick;
		//	entry.callback.AddListener((data) => ChangeScene(f._id, p._id, p));
		navPoint.GetComponent<EventTrigger>().triggers.Add (entry);
		navPoint.gameObject.name = tooltipTemp.type;  
		//point.transform.GetChild(0).GetComponent<TextMeshPro>().text =  (i+1).ToString();


		if (type == "VIDEO")
		{
            /*
            newButton.onClick.AddListener(() => LoadVideo(tooltipTemp));
            newButton.GetComponent<Image>().color = Color.clear;
            */
          
            navPoint.transform.Rotate(new Vector3(0, float.Parse(tooltipTemp.position[0].ToString()), 0), Space.Self);
            navPoint.transform.Translate(new Vector3(0, 0, 3.5f), Space.Self);
            entry.callback.AddListener((data) => LoadVideo(tooltipTemp));
			//point.gameObject.name = tooltipTemp.linkedPoiID;

		}
		if (type == "GATE")
		{
            /*
            newButton.name = tooltipTemp.linkedPoiID;
            newButton.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
            newButton.onClick.AddListener(() => ChangeScene(tooltipTemp.floorID,tooltipTemp.linkedPoiID, poiButtonTemp));
            newButton.GetComponent<Image>().color = new Color(0.70f, 0.70f, 0.70f, 1);
            */

				
				
			navPoint.transform.Rotate(new Vector3(0, 360 - float.Parse(tooltipTemp.position[0].ToString()), 0), Space.Self);
            navPoint.transform.Translate(new Vector3(0, 0, 3.5f), Space.Self);
            x1 = navPoint.transform.position.x;
            y1 = navPoint.transform.position.y;
            z1 = navPoint.transform.position.z;
            navPoint.transform.position = new Vector3(x1, -1.5f, z1);

            Floor itemFloor = centerTemp.floors.Find(x => x._id == tooltipTemp.floorID);
            Poi itemPoi = itemFloor.pois.Find(x => x._id == tooltipTemp.linkedPoiID);
            entry.callback.AddListener((data) => ChangeScene(tooltipTemp.floorID,tooltipTemp.linkedPoiID,itemPoi));
			navPoint.gameObject.name = tooltipTemp.linkedPoiID;
		}

		if (type == "PANELIMAGE")
		{
            /*
            newButton.onClick.AddListener(() => LoadPanelImage(tooltipTemp));
            newButton.GetComponent<Image>().color = new Color(0.01f, 0.44f, 0.051f, 0.5f);
            newButton.GetComponentInChildren<Text>().text = "";
            //newButton.GetComponent<Image>().color = Color.clear;
            */
          
            navPoint.transform.Rotate(new Vector3(0, float.Parse(tooltipTemp.position[0].ToString()), 0), Space.Self);
            navPoint.transform.Translate(new Vector3(0, 0, 3.5f), Space.Self);
            navPoint.transform.GetChild (0).GetComponent<TextMeshPro> ().text = "";
			entry.callback.AddListener((data) => LoadPanelImage(tooltipTemp));
			//point.gameObject.name = tooltipTemp.linkedPoiID;

		}

		if (type == "PANO")
		{
            /*
            newButton.onClick.AddListener(() => Load360(tooltipTemp));
            newButton.GetComponent<Image>().color = Color.clear;
            */
         
            navPoint.transform.Rotate(new Vector3(0, float.Parse(tooltipTemp.position[0].ToString()), 0), Space.Self);
            navPoint.transform.Translate(new Vector3(0, 0, 3.5f), Space.Self);
            entry.callback.AddListener((data) =>  Load360(tooltipTemp));
			//point.gameObject.name = tooltipTemp.linkedPoiID;

		}  


		if (type == "STEREOSCOPIC")
		{
            /*
            newButton.onClick.AddListener(() => ImageStereo(tooltipTemp));
            newButton.GetComponent<Image>().color = Color.clear;
            */
           
            navPoint.transform.Rotate(new Vector3(0, float.Parse(tooltipTemp.position[0].ToString()), 0), Space.Self);
            navPoint.transform.Translate(new Vector3(0, 0, 3.5f), Space.Self);
            entry.callback.AddListener((data) =>   ImageStereo(tooltipTemp));
		}


		if (type == "TEXTBLOCK")
		{
            /*
            newButton.onClick.AddListener(() => LoadTextBlock(tooltipTemp));
            newButton.GetComponent<Image>().color = Color.clear;
            */
           
            navPoint.transform.Rotate(new Vector3(0, float.Parse(tooltipTemp.position[0].ToString()), 0), Space.Self);
            navPoint.transform.Translate(new Vector3(0, 0, 3.5f), Space.Self);
            entry.callback.AddListener((data) =>    LoadTextBlock(tooltipTemp));

		}

	}





	private void BrowseChild(Tooltip tooltipTemp)
	{
		int childs;
		childs = tooltipAudioPanel.transform.transform.GetChildCount();        
		for (int i = 1; i < childs; i++)
		{

			if (tooltipTemp._id == tooltipAudioPanel.transform.GetChild(i).name )
			{
				Debug.Log("ESTE: " + tooltipTemp._id + " CON ESTE: " + tooltipAudioPanel.transform.GetChild(i).name);
				soundGuide = tooltipAudioPanel.transform.GetChild(i).gameObject;
				tooltipAudioPanel.transform.GetChild(i).gameObject.SetActive(true);
			}
		}
	}


	private void ImageStereo(Tooltip tooltipTemp)
	{
		BrowseChild(tooltipTemp);
		//audioPanel.transform.GetChild(1).gameObject.SetActive(true);

		SetAppState(AppStateType.STATE_APP_LOADING);
		imagePlane.transform.GetChild(1).gameObject.SetActive(true);
		MainCameraLeftTemp = GameObject.Find("Main Camera").transform.GetChild(1);
		MainCameraRightTemp = GameObject.Find("Main Camera").transform.GetChild(2);
		MainCameraLeftTemp.GetComponent<GvrEye>().toggleCullingMask = LayerMask.GetMask("Right_Eye_Layer");
		MainCameraRightTemp.GetComponent<GvrEye>().toggleCullingMask = LayerMask.GetMask("Left_Eye_Layer");


		MainCamera = GameObject.Find("Main Camera").transform.gameObject;
		MainCamera.GetComponent<StereoController>().UpdateStereoValues();

		ImagePlaneTempLeft = imagePlane.transform.GetChild(0).gameObject;
		ImagePlaneTempRight = imagePlane.transform.GetChild(1).gameObject;

		ImagePlaneTempLeft.layer = 8;
		ImagePlaneTempRight.layer = 9;

		StartCoroutine(DownloadImage(tooltipTemp.sourceUriLeft, "imagestereoleft"));
		StartCoroutine(DownloadImage(tooltipTemp.sourceUriRight, "imagestereoright"));
		if (tooltipTemp.text != "")
		{

			SetText(tooltipTemp.text);
		}       

	}


	private void AspectRatio(int widht, int height, GameObject objectTemp)
	{
		int aspectRatio = widht / height;

		objectTemp.transform.localScale = new Vector3(aspectRatio * objectTemp.transform.localScale.x, objectTemp.transform.localScale.y, objectTemp.transform.localScale.z);


		if (Camera.main.aspect >= 1.7)
		{
			Debug.Log("16:9");
		}
		else if (Camera.main.aspect >= 1.5)
		{
			Debug.Log("3:2");
		}
		else
		{
			Debug.Log("4:3");
		}

	}



	private void ManageAudio(string actionaudio, string url)
	{

		if (actionaudio == "play")
		{            
			audioMediaPlayer.Stop();
			audioMediaPlayer.Load(url);
			audioMediaPlayer.Play();

		}

		if (actionaudio == "stop")
		{
			audioMediaPlayer.Stop();          

		}
	}



	private void LoadTextBlock(Tooltip tooltipTemp)
	{

		BrowseChild(tooltipTemp);
		SetAppState(AppStateType.STATE_APP_LOADING);
		string text2write = tooltipTemp.text;
		Debug.Log(tooltipTemp.text);
		SetText(text2write);
		ManageAudio("play", tooltipTemp.soundGuide);
		//TODO This state call should be in the download image method...
		SetAppState(AppStateType.STATE_APP_TEXT);
	}


	private void LoadVideo(Tooltip tooltipTemp)
	{
		audioMediaPlayer.Stop();
		string urlvideo = tooltipTemp.sourceVideo;        
		SetAppState(AppStateType.STATE_APP_VIDEO);      
		videoPlane.transform.GetChild(0).GetComponent<MediaPlayerCtrl>().m_strFileName = urlvideo;


	}

	private void LoadPanelImage(Tooltip tooltipTemp)
	{
		BrowseChild(tooltipTemp);
		SetAppState(AppStateType.STATE_APP_LOADING);
		string uriImage = tooltipTemp.source;
		string text2write = tooltipTemp.text;
		imagePlane.transform.GetChild(1).gameObject.SetActive(false);  
		SetText(text2write);      
		ManageAudio("play", tooltipTemp.soundGuide);
		StartCoroutine(DownloadImage(uriImage, "image"));    

	}


	public void Load360(Tooltip tooltipTemp)
	{
		BrowseChild(tooltipTemp);
		Debug.Log("la escena previa es" + prevPoiID);
		SetAppState(AppStateType.STATE_APP_LOADING);
		string url360 = tooltipTemp.attributionUri;
		StartCoroutine(DownloadImage(url360, "360"));

	}





	public void SetText(string textToWrite)
	{
		textContainer.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = textToWrite;
		imagePlane.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = textToWrite;
		imagePlane.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = textToWrite;
	}






	// Update is called once per frame
	void Update()
	{





		switch (currentState)
		{

		case AppStateType.STATE_APP_IDLE:
			StateIdleUpdate();
			break;
		case AppStateType.STATE_APP_IDLEBTN:
			StateIdleBtnUpdate();
			break;
		case AppStateType.STATE_APP_IMAGE:
			StateImageUpdate();
			break;
		case AppStateType.STATE_APP_LOADING:
			StateLoadingUpdate();
			break;
		case AppStateType.STATE_APP_VIDEO:
			StateVideoUpdate();
			break;
		case AppStateType.STATE_APP_TEXT:
			StateTextUpdate();
			break;
		case AppStateType.STATE_APP_MENU:
			StateMenuUpdate();
			break;
		case AppStateType.STATE_APP_360:
			State360Update();
			break;        



		}

	}

	public void StateIdleUpdate()
	{
		
		if (MainCamera.transform.localRotation.eulerAngles.x > 30.0f && MainCamera.transform.localRotation.eulerAngles.x < 50.0f)  				 //Show menu button
		{

			SetAppState (AppStateType.STATE_APP_IDLEBTN);

		}


	}
	public void StateIdleBtnUpdate()
	{

	}

	public void State360Update()
	{

		if (Input.anyKey)
		{

			if (!EventSystem.current.IsPointerOverGameObject())
			{
				GoBack();                                            //In this state we need to load the scene again, that's why we call the GoBack() function instead of changing state.
				SetAppState(AppStateType.STATE_APP_LOADING);
			}

		}

	}

	public void StateImageUpdate()
	{
		if (Input.anyKey)
		{
			if (!EventSystem.current.IsPointerOverGameObject())
			{
				SetAppState(AppStateType.STATE_APP_IDLE);
				imagePlane.SetActive(false);
			}
		}

	}
	public void StateLoadingUpdate()
	{


	}
	public void StateTextUpdate()
	{
		if (Input.anyKey)
		{
			if (!EventSystem.current.IsPointerOverGameObject())
			{
				SetAppState(AppStateType.STATE_APP_IDLE);
				textContainer.SetActive(false);
			}
		}
	}
	public void StateNavigationUpdate() { }

	public void StateMenuUpdate() { }
	public void StateVideoUpdate()
	{

		if (Input.anyKey)
		{
			if (!EventSystem.current.IsPointerOverGameObject())
			{
				SetAppState(AppStateType.STATE_APP_IDLE);
				videoPlane.SetActive(false);
			}
		}

	}


	public void GoBack()
	{



		//SetAppState(AppStateType.STATE_APP_IDLE);

	}

	//This method loads images form the strip NOT the actual panel image

	//Used to show/hide the menu button using an invisible collider located down.
	public void ShowMenuButton()
	{

		showMenuButton.SetActive (true);
		//showMenuButton.transform.position = new Vector3 (0,0,0);
		//showMenuButton.transform.Rotate (new Vector3 (0, 0, 0));
		showMenuButton.transform.SetParent (null);
		StartCoroutine ("Hide");

	}

	/*public void HideMenuButton()
	{
		//StartCoroutine ("Hide");
	}*/

	//Called when click on the button to open menu
	public void ShowMenu()
	{
		showMenuButton.SetActive (false);
		if (currentState == AppStateType.STATE_APP_MENU)
		{
			SetAppState(AppStateType.STATE_APP_IDLE);
		}

		else

		{
			SetAppState(AppStateType.STATE_APP_MENU);
		}
	}

	IEnumerator Hide()  // Hides menu button and resets rotation so it can reappear at the correct position relative to the camera
	{

		yield return new WaitForSeconds (5.0f);
		//showMenuButton.transform.position = new Vector3 (0,0,0);
		showMenuButton.transform.SetParent (referenceObject.transform);

		showMenuButton.transform.localRotation = Quaternion.identity;
		showMenuButton.transform.Rotate (new Vector3 (90,-90,0));
		showMenuButton.SetActive (false);

		if (currentState == AppStateType.STATE_APP_MENU)
		{
			//SetAppState(AppStateType.STATE_APP_IDLE);
		}

		else

		{
			SetAppState(AppStateType.STATE_APP_IDLE);
		}

	}

	public void LoadCategories()
	{


		categories = new List<string>();
	
		pieces = new List<Tooltip>();
		//dictCategories = new Dictionary<object, List<string>>();

		foreach (Floor f in centerTemp.floors)
		{
			foreach (Poi p in f.pois) 
			{

				foreach (Tooltip t in p.tooltips) 
				{

					if (t.category != null ) 
					{
						pieces.Add(t);
						foreach (string c in t.category)
						{

							if (c != null && !categories.Contains(c)) 
							{

								categories.Add (c);
								//print (c + " Added");
							}

						}

					}
				}

			}

		}

		/*
        foreach (DictionaryEntry item in categories)
        {
            Debug.Log(item.Key);
            Tooltip temp = (Tooltip) item.Key;
            List<string> temp2 = (List<string>)item.Value;
            Debug.Log(temp2[0]);
        }

      */ 


	}

	IEnumerator Reload360Image(Poi poitempz)
	{


		string tempUrl = serverUrl + poitempz.uri;
		Debug.Log("Se descargara url:" + tempUrl);
		WWW www = new WWW(tempUrl);
		yield return www;

		if (!string.IsNullOrEmpty(www.error))
		{
			//SetAppState(AppStateType.STATE_APP_LOADING);
			//loadingMessage.GetComponent<Text>().text = "Compruebe su conexión a internet";
			Debug.Log("Check internet conection " + www.error);

			while (!string.IsNullOrEmpty(www.error))
			{

				www = new WWW(poitempz.uri);
				yield return www;
			}

		}

		Texture2D tex;
		tex = new Texture2D (www.texture.width,www.texture.height , TextureFormat.ETC2_RGB, false);

		www.LoadImageIntoTexture(tex);

		this.gameObject.GetComponent<Renderer>().material.mainTexture = tex; 

		savedImages.Add(tex);
		print ("Map Loaded from web");

		byte[] bytes = tex.EncodeToJPG(90);

		File.WriteAllBytes (Application.dataPath + "/Resources/"+ poitempz._id +".jpg",bytes);



	}


}





