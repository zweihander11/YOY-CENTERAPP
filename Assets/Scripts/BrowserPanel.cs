using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrowserPanel : MonoBehaviour 
{

	GenericMenu genericMenu;
	public GameObject piecePrefab;

	void Start() 
	{

		genericMenu = this.transform.parent.transform.parent.GetComponent<GenericMenu>();


	}
	
	public void InitializePanel()
	{

		foreach (Tooltip tt in genericMenu.intersectTooltip) 
		{

			print ("Broser Initialized");
			Instantiate (piecePrefab, genericMenu.rightPanel.transform, false);



		}




	}




}
