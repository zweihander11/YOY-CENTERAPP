using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripNavigator : MonoBehaviour 
{

	// Automatically creates buttons for navigation in a Strip Object

	public GameObject _canvas;
	public int imgAmount;
	public float margin;
	public float offset;

	//private GameObject[] btnPrefab;



	void Start()
	{
		
		for (int i = 0; i < imgAmount; i++) 
		{
			
			Instantiate (_canvas, this.transform); 

		}

		foreach (Transform child in transform) 
		{
			
			child.transform.Rotate (0, child.GetSiblingIndex() * margin, 0);

		}


	}
	

	void Update()
	{
		
	}
}
