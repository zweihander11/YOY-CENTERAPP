using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLerp : MonoBehaviour 
{


	public Transform from;
	public Transform to;
	public float speed = 0.1F;

	private float newRot;

	void Update()
	{
		
		//newRot = to.rotation.y;
		//transform.rotation = Quaternion.Lerp(new Quaternion (0,from.rotation.y,0,0), new Quaternion (0,to.rotation.y,0,0), Time.time * speed);
		//transform.rotation.eulerAngles = Vector3.Lerp(new Vector3 (0,from.rotation.y,0), new Vector3 (0,to.rotation.y,0),Time.time * speed) ;

		//transform.LookAt(to);
		newRot = Mathf.LerpAngle(from.rotation.y,to.rotation.y,Time.time * speed);
		//print (newRot);

		if( from.rotation != to.rotation )
		{

			transform.Rotate (new Vector3 (0, newRot ,0), Space.Self);

		}
	}


}
