using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.IO;
using UnityEngine.EventSystems;




public class ControllerMap : MonoBehaviour 
{



   	GenericMenu genericMenu;                                        //Reference to script GenericMenu 
  	GameObject ObjectFloorMap;                                      //GameObject created for child of Map
  	public GameObject pointPrefab;                                  //Prefab of point in the map
  	public GameObject buttonPrefab; 				// A prefab for interactable buttons. Ex: Floor buttons

	public GameObject pointContainer; 				// Reference to the point container Game Object

	private GameObject currentActivePoi;				//Reference to the GameObject of the POI in map

	public GameObject attachLevelbtns; 				// A gameobject in this prefab that serves as anchor for the creation of level buttons
	public GameObject levelbtnPrefab;  				// A prefab or game object to show as level button
	public List<GameObject> levelButtons;				// A collection of levels button generated from json file 

	private Texture2D tex;  					// Declaration to hold temporary downloaded textures

	public List<Texture2D> savedMaps = new List<Texture2D>();	// A list where we store downloaded map textures

 



    public void CalltoScript() 										// Called from outside when all refrences are present 
    {

	genericMenu = this.transform.parent.transform.parent.GetComponent<GenericMenu>();
    	Floor itemFloor = genericMenu.appController.centerTemp.floors.Find(x => x._id == genericMenu.appController.currentFloor._id);
	
	
        LoadMapinPanel(itemFloor);

		foreach (Floor fl in  genericMenu.appController.centerTemp.floors)  // We add a texture slot for each floor in tthis Center
		{
		
			savedMaps.Add (null);
		}

		StartCoroutine(ReloadMapImage( itemFloor));  						// Load map image for starting floor
		
		ShowLevelButtons (); 								// Load POIs on map for this floor
		genericMenu.appController.LoadCategories();
		print ("Called script" );
		genericMenu.PopulateCategories ();
    }


    
    private void LoadMapinPanel(Floor itemFloor) 							// Shows POIs on map according to current floor
    {

        int i = 0;

        Poi locatedPoi = itemFloor.pois.Find(x => x._id == genericMenu.appController.currentPoi._id);

		foreach (Poi poiItem in itemFloor.pois)
      		{    			

			GameObject point = Instantiate(pointPrefab, pointContainer.transform, false); 		// Changed the parent to an object in scene
			point.name = poiItem._id;		
			
			point.transform.Rotate(new Vector3( 0, poiItem.position[0] * 0.055f , 0), Space.Self); //TODO put this multiplier as an exposed variable
			point.transform.Translate(new Vector3(0, poiItem.position[1] * 0.0095f , 0), Space.Self);
			
			point.AddComponent<EventTrigger> ();		


			foreach (Tooltip t in poiItem.tooltips) 
			{

				if (t.type.Equals ("GATE"))
				{

					EventTrigger.Entry entry = new EventTrigger.Entry ();
					entry.eventID = EventTriggerType.PointerClick;
					entry.callback.AddListener ((data) => genericMenu.appController.ChangeScene (t.floorID, t.linkedPoiID, poiItem));
					point.GetComponent<EventTrigger> ().triggers.Add (entry);	

					EventTrigger.Entry entry2 = new EventTrigger.Entry ();
					entry2.eventID = EventTriggerType.PointerClick;

					entry2.callback.AddListener ((data) => SwitchtoCurrent (point));
					point.GetComponent<EventTrigger> ().triggers.Add (entry2);

				}
		

			}	
		
			if (poiItem._id.Equals(genericMenu.appController.currentPoi._id))
				{
				// Update "you are here" image

					point.transform.GetChild (0).gameObject.SetActive (false);
					point.transform.GetChild (1).gameObject.SetActive (true);
					currentActivePoi = point.gameObject;
				}

				i++;
        }
		
    }

    private void CreateFloorObject()
    {


        ObjectFloorMap = new GameObject("AttachPoiToMap");
		ObjectFloorMap.transform.SetParent(genericMenu.centralPanel.transform.GetChild(0).transform.GetChild(1).transform,false);
		

    }

	public void ChangeFloorMap(Floor floorTemp)                                //Loads a new floor when the floor map button is pressed
    {

	 
		Destroy (pointContainer);

		pointContainer = new GameObject ();
		pointContainer.transform.SetParent( genericMenu.centralPanel.transform, true);
		pointContainer.transform.localPosition = new Vector3 (0,0,0);
		pointContainer.transform.localRotation = new Quaternion (0,0,0,0);
		pointContainer.transform.Translate (new Vector3(0,-2.73f,0));
		pointContainer.transform.Rotate (new Vector3 (0,-40,0));
			
		pointContainer.name = "PointContainer";
		LoadMapinPanel(floorTemp); 


		if ( savedMaps[floorTemp.floorNumber] == null )  // If this map image is not stored on the list yet, download it
		{
		
			StartCoroutine (ReloadMapImage (floorTemp));

		} 
		else 
		{	

			this.gameObject.GetComponent<Renderer> ().material.mainTexture = savedMaps [floorTemp.floorNumber];
			print ("Map Loaded from disk");

		}


    }


    private void GenerateButtonSelectFloor()        //Method for create button to select Floor in the map TODO: deprecated
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



	public void SwitchtoCurrent(GameObject point)      // Switches visual mark on map for 'You are here' purpose. 
	{


		currentActivePoi.transform.GetChild (0).gameObject.SetActive (true);
		currentActivePoi.transform.GetChild (1).gameObject.SetActive (false);
		point.transform.GetChild (0).gameObject.SetActive (false);
		point.transform.GetChild (1).gameObject.SetActive (true);
		currentActivePoi = point.gameObject;


	}

	public void SwitchfromNav(string id)  			// Switches visual mark on map for 'You are here' purpose. Called during scene navigation
	{

		currentActivePoi.transform.GetChild (0).gameObject.SetActive (true);
		currentActivePoi.transform.GetChild (1).gameObject.SetActive (false);
		currentActivePoi = pointContainer.transform.FindChild(id).gameObject;
		currentActivePoi.transform.GetChild (0).gameObject.SetActive (false);
		currentActivePoi.transform.GetChild (1).gameObject.SetActive (true);

	}

	public void ShowLevelButtons() // Reads number of floors and creates buttons for each of them. TODO: Dinamically load +5 floors
	{


		int i = 0;

		foreach (Floor itemFloor in genericMenu.appController.centerTemp.floors) 
		{
			
			GameObject lvlbtn = Instantiate (levelbtnPrefab, genericMenu.centralPanel.transform);
			lvlbtn.transform.Rotate (genericMenu.transform.rotation.eulerAngles);
			lvlbtn.transform.Rotate (new Vector3(0,-25,0));
			lvlbtn.transform.Translate (0,-(i-0.6f),0);
			lvlbtn.transform.GetChild (0).GetComponent<TextMeshPro>().text = itemFloor.floorNumber.ToString();

			lvlbtn.AddComponent<EventTrigger> ();
			EventTrigger.Entry entry= new EventTrigger.Entry ();
			entry.eventID = EventTriggerType.PointerClick;

			entry.callback.AddListener ((data) => ChangeFloorMap(itemFloor));

			lvlbtn.GetComponent<EventTrigger>().triggers.Add (entry);	

			levelButtons.Add (lvlbtn);
			i++;
		}


	}



	public IEnumerator ReloadMapImage(Floor fl)
	{
		
		string tempUrl = genericMenu.appController.serverUrl + fl.uriMap;
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

				www = new WWW(fl.uriMap);
				yield return www;
			}

		}

		Texture2D tex;
		tex = new Texture2D (www.texture.width,www.texture.height , TextureFormat.ETC2_RGB, false);

		www.LoadImageIntoTexture(tex);

		this.gameObject.GetComponent<Renderer>().material.mainTexture = tex; 
			
		savedMaps[fl.floorNumber] = tex;
		print ("Map Loaded from web");

		byte[] bytes = tex.EncodeToJPG();

		File.WriteAllBytes (Application.dataPath + "/Resources/Mapas/"+ fl._id +".jpg",bytes);

	}


}
