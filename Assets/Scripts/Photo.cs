 using LitJson;
using UnityEngine;
using System.Collections.Generic;
public class Photo{

    public int photo_id;
    public float rotationOffset;
    public string uri;
    public string scaudio;
    public List<Tooltip> tooltipList; 
    
    public Photo(KeyValuePair<string, JsonData> singlePhoto){                                   //Constructor de la clase Photo
        this.photo_id = int.Parse(singlePhoto.Key);
        this.rotationOffset = float.Parse(singlePhoto.Value["rotationOffset"].ToString());
        this.uri = singlePhoto.Value["uri"].ToString();
        this.scaudio = singlePhoto.Value["scaudio"].ToString();
        tooltipList = this.getTooltip(singlePhoto.Value["tooltips"]);
    }

    public List<Tooltip> getTooltip(JsonData tooltips) {
        List<Tooltip> tempList = new List<Tooltip>();        
        foreach (JsonData tool in tooltips){
            Tooltip prueba = JsonUtility.FromJson<Tooltip>(tool.ToJson());   
            tempList.Add(prueba);
        }
        return tempList;
    }
}
