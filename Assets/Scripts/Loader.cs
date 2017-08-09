using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour 
{

	//TODO place booleans to activate or deactivate preloader screen
	public bool onlyCardBoard;
	public bool onlyPhoneMode;

	[HideInInspector]public bool vrMode;

	public string nextScene;

	void Start()
	{

		DontDestroyOnLoad (this.gameObject);
		if(onlyCardBoard && !onlyPhoneMode) // Check and load automatically
		{
			vrMode = true;
			SceneManager.LoadScene (nextScene);

		}

		if(!onlyCardBoard && onlyPhoneMode) // Check and load automatically
		{
			vrMode = false;
			SceneManager.LoadScene (nextScene);

		}
	
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
