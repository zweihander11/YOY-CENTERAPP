using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideButton : MonoBehaviour 
{

	CreateMap createMap;

	//Deprecated
	public GameObject appearCol;

	void Start()
	{

		createMap = GameObject.Find ("sphere3").GetComponent<CreateMap>();

	}
	/*
	public void Show ()
	{
		 Deprecated as we called them form create map script
		this.gameObject.SetActive (true);
		this.transform.SetParent (null);
		StartCoroutine ("Hide");

	}
	
	IEnumerator Hide()
	{

		/* Deprecated as we called them form create map script
		yield return new WaitForSeconds (2.0f);
		this.gameObject.SetActive (false);

	}
*/
	public void ShowMenu ()
	{
		createMap.menuContainer.SetActive (true);
		//this.transform.SetParent (null);
		//appearCol.SetActive(false);

		//StartCoroutine ("Hide");


	}

	public void CloseMenu()
	{

		createMap.menuContainer.SetActive (false);
		createMap.SetAppState (CreateMap.AppStateType.STATE_APP_IDLE);

	}



}
