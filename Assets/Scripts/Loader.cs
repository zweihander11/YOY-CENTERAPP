using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour 
{

	public bool vrMode;

	public string nextScene;

	void Start()
	{

		DontDestroyOnLoad (this.gameObject);
	}


	public void ModeSelection(bool cardboard)  		// Function called form splash canvas to switch between Cardboard or Simple Mode
	{


		if (cardboard) 
		{

			vrMode = true;


		} 
		else
		{

			vrMode = false;



		}

		SceneManager.LoadScene (nextScene);

	}
}
