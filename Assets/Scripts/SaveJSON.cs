using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class SaveJSON : MonoBehaviour {    
    JsonData Data;
    public GameObject sphere3;


    

    // Update is called once per frame
    public void ConvertToJson(Center center)
    {
        Data = JsonUtility.ToJson(center);
        //Data = JsonMapper.ToJson(center);
       
        File.WriteAllText(Application.dataPath + "CultureApp.json", Data.ToString());
        Debug.Log(Data.ToString());
        Debug.Log("JSON Generated Correctly");
    }
}
