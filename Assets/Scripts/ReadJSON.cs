using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReadJSON : MonoBehaviour
{
    public bool end = false;
    public GameObject sphere3;
    public JsonData ItemData;
    public Scene scene;
    private Photo photo;
    public string oriPath;
 
   

    // Use this for initialization
    void Start(){       
        if (Application.isMobilePlatform)
        {
            StartCoroutine("GetJsonData");
        }
        else if (Application.isEditor)
        {
            if (File.Exists(Application.streamingAssetsPath + "/Info_JSON.json") )
            {
                Debug.Log("Exists");
            }
            ItemData = JsonMapper.ToObject(File.ReadAllText(Application.streamingAssetsPath + "/Info_JSON.json"));
            Construct();
        }
       
    }



    IEnumerator GetJsonData(){
        WWW www = new WWW(Application.streamingAssetsPath + "/Info_JSON.json");        
        while (!www.isDone)
        {
            yield return www;
            File.WriteAllBytes(Application.persistentDataPath + "/Info_JSON.json", www.bytes);
        }       
        ItemData = JsonMapper.ToObject(File.ReadAllText(Application.persistentDataPath + "/Info_JSON.json"));
        Construct();
    }


    
    private void Construct() {
      
        Scene scene = new Scene();
        scene.nav_icon = ItemData["nav_icon"].ToString();                                           //Se rescata las primeras variables del JSON para ser cargadas en el objeto
        scene.firstPhotoId = int.Parse(ItemData["firstPhotoId"].ToString());
        scene.firstPhotoRotation = float.Parse(ItemData["firstPhotoRotation"].ToString());

        foreach (KeyValuePair<string, JsonData> singlePhoto in ItemData["photos"])                    //Iteracion para ir guardando cada objeto foto en la lista de fotos
        {                                                                                  
            Photo tempPhoto = new Photo(singlePhoto);
            scene.photosList.Add(tempPhoto);
        }


        this.scene = scene;
        end = true;                                                                              //Boolean para determinar que el script termino de leer el JSON y cargarlo   



    }
}

