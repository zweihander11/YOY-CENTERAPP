using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour 
{


	void Update()
	{


		this.gameObject.transform.Rotate( new Vector3(0, Time.maximumDeltaTime * -0.2f, 0));



	}


}
