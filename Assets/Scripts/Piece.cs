using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour 
{

	public Texture2D pieceTexture;
	public string pieceTextureURL;
	public GameObject camera;
	public GameObject lookAtTarget;
	public GameObject pieceNormal;

	private float rotX;

	void Start() 
	{

		camera = GameObject.FindGameObjectWithTag ("MainCamera");

	}


	void Update()
	{
		
		
	//	print ( Mathf.Atan (lookAtTarget.transform.position.z / (Vector3.Distance (lookAtTarget.transform.position, this.GetComponent<MeshCollider> ().bounds.center))));
	/*	if (  Mathf.Atan (lookAtTarget.transform.position.z / (Vector3.Distance (lookAtTarget.transform.position, this.GetComponent<MeshCollider> ().bounds.center))) < 2
			&& Mathf.Atan (lookAtTarget.transform.position.z / (Vector3.Distance (lookAtTarget.transform.position, this.GetComponent<MeshCollider> ().bounds.center)))> -2
		)
		{
			
			transform.RotateAround (this.GetComponent<MeshCollider> ().bounds.center, new Vector3 (0, 1, 0), Mathf.Atan (lookAtTarget.transform.position.z / (Vector3.Distance (lookAtTarget.transform.position, this.GetComponent<MeshCollider> ().bounds.center))));
	
		}

*/


	}

	IEnumerator DownloadImage(string url, string type)                   //Metodo para descargar algo de internet. Imagenes, texto etc... 
	{         



		string tempUrl = transform.parent.GetComponent<GenericMenu>().appController.serverUrl + url;
		Debug.Log ("URL Obra:" + tempUrl);
		WWW www = new WWW (tempUrl);
		yield return www;
		if (!string.IsNullOrEmpty (www.error)) 
		{
			
			while (!string.IsNullOrEmpty (www.error)) 
			{
				www = new WWW (url);
				yield return www;
			}
		}
	}


}
