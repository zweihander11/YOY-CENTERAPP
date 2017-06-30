using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTarget : MonoBehaviour {
    
    public static MouseTarget mInstance;
    public static MouseTarget instance { get { return mInstance; } }
    public SpriteRenderer targetSprite;

    // Use this for initialization
    void Start () {
        if (mInstance == null)
        {
            mInstance = this;
        }
		
	}

    public void TargetChoosen(bool choosen){
        if (choosen)
        {
            targetSprite.color = Color.white;
        }else
        {
            targetSprite.color = Color.red;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
