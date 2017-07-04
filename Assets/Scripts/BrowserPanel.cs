using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BrowserPanel : MonoBehaviour 
{

	GenericMenu genericMenu;
	public GameObject piecePrefab;

	private int displayPieceCount =  12;  		// The amount of art pieces to diplay on the menu. hard coded acording to the amount fitting the menu.



	void Start() 
	{

		genericMenu = this.transform.parent.transform.parent.GetComponent<GenericMenu>();


	}


	
	public void InitializePanel()
	{
		print ("Browser Initialized " +  genericMenu.intersectTooltip.Count);
		if (genericMenu.intersectTooltip.Count < displayPieceCount) 			// If there's only 1 page
		{


			foreach (Tooltip tt in genericMenu.intersectTooltip) 
			{

				for (int i = genericMenu.intersectTooltip.Count; i <= displayPieceCount; i++) // Hide the remaining pieces
				{
					if (i != 0 && i < displayPieceCount) 
					{
						print (genericMenu.intersectTooltip.Count + " I:" + i);
						genericMenu.rightPanel.transform.GetChild (i+1).gameObject.SetActive (false);
					}


				}


				foreach (Transform obj in genericMenu.rightPanel.transform)
				{

					if (obj.gameObject.tag == "piece" && obj.gameObject.activeSelf) 
					{

						print ("AddedPiece Listener");
						obj.gameObject.AddComponent<EventTrigger>();
						EventTrigger.Entry entrypiece = new EventTrigger.Entry ();
						entrypiece.eventID = EventTriggerType.PointerClick;
						entrypiece.callback.AddListener ((data) =>  genericMenu.appController.InitScene(tt.poiID, tt.floorID) );
						obj.gameObject.GetComponent<EventTrigger> ().triggers.Add (entrypiece);



					}


				}

				//TODO add support for extra pages
				//TODO add support for thumbnail show
			}










		}
	}




}
