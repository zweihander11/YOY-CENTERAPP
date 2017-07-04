using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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





			}

			for (int i = genericMenu.intersectTooltip.Count; i <= displayPieceCount; i++) // Hide the remaining pieces
			{


				genericMenu.rightPanel.transform.GetChild (i).gameObject.SetActive(false);
			

			}



		}
	}




}
