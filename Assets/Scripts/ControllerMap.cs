using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

using UnityEngine.EventSystems;




public class ControllerMap : MonoBehaviour 
{



    GenericMenu genericMenu;                                            //Reference to script GenericMenu 
    GameObject ObjectFloorMap;                                         //GameObject created for child of Map
    public GameObject pointPrefab;                                     //Prefab of point in the map
    public GameObject buttonPrefab; 									// A prefab for interactable buttons. Ex: Floor buttons
    private string currentPoi;
	public GameObject pointContainer;

	//public Vector3[] planeVertices;

	private GameObject currentActivePoi;

	public GameObject attachLevelbtns; 							// A gameobject in this prefab that serves as anchor for the creation of level buttons
	public GameObject levelbtnPrefab;  							// A prefab or game object to show as level button
	public List<GameObject> levelButtons;						// A collection of levels button generated from json file 

    void Start() 
	{
       


       
    }
	

	void Update()
	{
		if (currentPoi != genericMenu.appController.currentPoi._id)
        {
			currentPoi = genericMenu.appController.currentPoi._id;
			SwitchfromNav(genericMenu.appController.currentPoi._id);
        }
	}



    public void CalltoScript()
    {
		genericMenu = this.transform.parent.transform.parent.GetComponent<GenericMenu>();
       // Debug.Log("Hola");

		Floor itemFloor = genericMenu.appController.centerTemp.floors.Find(x => x._id == genericMenu.appController.currentFloor._id);
      //  CreateFloorObject();
      //  GenerateButtonSelectFloor();
        LoadMapinPanel(itemFloor);
		//ShowLevelButtons ();                  //Comentado para demo chilquinta
    }
    
    private void LoadMapinPanel(Floor itemFloor)
    {
        int i = 0;

        Poi locatedPoi = itemFloor.pois.Find(x => x._id == genericMenu.appController.currentPoi._id);
	//	planeVertices = this.GetComponent<MeshFilter> ().mesh.vertices;
	foreach (Poi poiItem in itemFloor.pois)
        {

			currentPoi = genericMenu.appController.currentPoi._id;
            GameObject point = Instantiate(pointPrefab, pointContainer.transform, false); // Changed the parent to an object in scene
			point.name = poiItem._id;		
			//GameObject point = Instantiate(pointPrefab, genericMenu.centralPanel.transform.GetChild(0).transform.GetChild(1).transform, false); // Changed the parent to an object in scene
			//GameObject point = new GameObject();
			//point.transform.parent = genericMenu.centralPanel.transform.GetChild (0).transform.GetChild (1).transform;
		
			//genericMenu.centralPanel.transform.GetChild(0).transform.GetChild(1).transform.localPosition = new Vector3(5,0,6);
			//point.transform.position = new Vector3( 0 , 0 , 0  );
			//point.transform.parent = pointContainer.transform;
			//	point.AddComponent<MegaAttach>();
			//point.GetComponent<MegaAttach>().target = genericMenu.centralPanel.transform.GetChild(0).GetComponent<MegaModifyObject>();
			//point.GetComponent<MegaAttach>().DetachIt();
			point.transform.Rotate(new Vector3( 0, poiItem.position[0] * 0.055f , 0), Space.Self); //TODO put this multiplier as an exposed variable
			point.transform.Translate(new Vector3(0, poiItem.position[1] * 0.0095f , 0), Space.Self);
			//point.transform.Rotate(new Vector3( 90 , 0 , 180  ));
			//point.GetComponent<MegaAttach>().AttachIt (point.transform.position);
			//point.GetComponent<MegaAttach>().DetachIt();

			point.AddComponent<EventTrigger> ();
			/*EventTrigger.Entry entry = new EventTrigger.Entry ();
			entry.eventID = EventTriggerType.PointerClick;

			entry.callback.AddListener ((data) => genericMenu.appController.InitScene (poiItem._id, poiItem.floorID));
			point.GetComponent<EventTrigger>().triggers.Add (entry);
			*/


		
			//entry.callback.AddListener ((data) => genericMenu.appController.InitScene (poiItem._id, poiItem.floorID));
		



		    

		        EventTrigger.Entry entry = new EventTrigger.Entry ();
		        entry.eventID = EventTriggerType.PointerClick;
		        entry.callback.AddListener ((data) => genericMenu.appController.ChangeScene(poiItem.floorID, poiItem._id, poiItem));
                point.GetComponent<EventTrigger> ().triggers.Add (entry);	
		        EventTrigger.Entry entry2 = new EventTrigger.Entry ();
		        entry2.eventID = EventTriggerType.PointerClick;
		        entry2.callback.AddListener ((data) => SwitchtoCurrent (point));
		        point.GetComponent<EventTrigger> ().triggers.Add (entry2);

		    		

		

			
		
		if (poiItem._id.Equals(genericMenu.appController.currentPoi._id))
			{
			// Update "you are here" image

				point.transform.GetChild (0).gameObject.SetActive (false);
				point.transform.GetChild (1).gameObject.SetActive (true);
				currentActivePoi = point.gameObject;
			}

			i++;



        }

		//genericMenu.centralPanel.transform.GetChild (0).transform.GetChild (1).transform.localPosition = new Vector3 (0,0,0);
    }

    private void CreateFloorObject()
    {
        ObjectFloorMap = new GameObject("AttachPoiToMap");
		ObjectFloorMap.transform.SetParent(genericMenu.centralPanel.transform.GetChild(0).transform.GetChild(1).transform,false);
		

    }

	public void ChangeFloorMap(Floor floorTemp)                                                                         //Metodo for change Floor in the map
    {

		print ("Change");

	Destroy (pointContainer);
		
		pointContainer = new GameObject ();
	pointContainer.transform.SetParent( genericMenu.centralPanel.transform, false);
		pointContainer.transform.position = new Vector3 (0,0,0);
		pointContainer.name = "PointContainer";
		//cargar pois del piso
        //Floor itemFloor = genericMenu.appController.centerTemp.floors.Find(x => x._id == floorTemp._id);
	//Floor itemFloor = genericMenu.appController.centerTemp.floors[floorNumb];	
	//print( genericMenu.appController.centerTemp.floors[floorNumb].floorNumber.ToString());
	LoadMapinPanel(floorTemp); 

    }


    private void GenerateButtonSelectFloor()                                                                             //Metod for create button to select Floor in the map
    {
        /*
        foreach (Floor itemFloor in genericMenu.appController.centerTemp.floors)
        {
            GameObject floorbtn = Instantiate(buttonPrefab, genericMenu.centralPanel.buttonPlacer., false);
            floorbtn.transform.Translate(0, fl.floorNumber * -1, 0);
            floorbtn.transform.GetChild(0).GetComponent<TextMeshPro>().text = (fl.floorNumber + 1).ToString();
            floorbtn.SetActive(true); 								 // Activate the button since the prefab must be inactive in order to exist in the scene prior this step
        }
        */
    }



	public void SwitchtoCurrent(GameObject point)
	{


		currentActivePoi.transform.GetChild (0).gameObject.SetActive (true);
		currentActivePoi.transform.GetChild (1).gameObject.SetActive (false);
		point.transform.GetChild (0).gameObject.SetActive (false);
		point.transform.GetChild (1).gameObject.SetActive (true);
		currentActivePoi = point.gameObject;


	}

	public void SwitchfromNav(string id)
	{

		


		currentActivePoi.transform.GetChild (1).gameObject.SetActive (false);
        currentActivePoi.transform.GetChild(0).gameObject.SetActive(true);

        currentActivePoi = pointContainer.transform.FindChild(id).gameObject;

		currentActivePoi.transform.GetChild (0).gameObject.SetActive (false);
		currentActivePoi.transform.GetChild (1).gameObject.SetActive (true);
	}

	public void ShowLevelButtons()
	{


	        int i = 0;

	        foreach (Floor itemFloor in genericMenu.appController.centerTemp.floors) 
	        {
		
	        GameObject lvlbtn = Instantiate (levelbtnPrefab, genericMenu.centralPanel.transform);
	        lvlbtn.transform.Rotate (genericMenu.transform.rotation.eulerAngles);
	        lvlbtn.transform.Rotate (new Vector3(0,-25,0));
	        //lvlbtn.transform.Translate (0,4,0);
	        lvlbtn.transform.Translate (0,-(i-0.6f),0);
	        lvlbtn.transform.GetChild (0).GetComponent<TextMeshPro>().text = itemFloor.floorNumber.ToString();

	        print (itemFloor.floorNumber );
	        lvlbtn.AddComponent<EventTrigger> ();
	        EventTrigger.Entry entry= new EventTrigger.Entry ();
	        entry.eventID = EventTriggerType.PointerClick;
	        print (itemFloor);		
	        entry.callback.AddListener ((data) => ChangeFloorMap(itemFloor));

	        lvlbtn.GetComponent<EventTrigger>().triggers.Add (entry);	

	        levelButtons.Add (lvlbtn);
	        i++;
		        }


	}


}
