using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetJSON : MonoBehaviour
{
    public bool end = false;
    public GameObject sphere3;
    public JsonData ItemData;
    public Center center;
    private List<Poi> poisTemp;
    private List<Floor> floorsTemp;
    private List<Tooltip> tooltipsTemp;
    private SaveJSON saveJSON;

    static GetJSON instance = null;                               //Set a singleton so this object persists and can be accessed from different scripts

    public static GetJSON Instance
    {
        get
        {

            if (instance == null)
            {
                instance = (GetJSON)FindObjectOfType(typeof(GetJSON));
            }
            return instance;

        }
    }

   


    // Use this for initialization
    void Start()
    {
        saveJSON = GameObject.Find("sphere3").GetComponent<SaveJSON>();
        ConnectToServer();     
        
    }

    void ConnectToServer()
    {
        string url = "http://192.168.0.103:5000/graphql";
        string ourPostData = "{ \"query\": \"query{ center{_id firstPoiID  firstFloorID   firstPoiRotation floors{ _id  floorNumber  pois{ _id uri  soundGuide rotationOffset tooltips{ _id type poiID position text soundGuide attribution  attributionUri width  height ...on TextBlock { title   } ... on Stereoscopic { sourceUriLeft  sourceUriRight } ... on Gate { linkedPoiID  } ... on PanelImage { source } ... on Video { sourceVideo   } ... on Pano{ sourcePano  } } } } } } \"}";
        byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");
        WWW api = new WWW(url, pData, headers);
        StartCoroutine(WaitForRequest(api));
        
    }


    IEnumerator WaitForRequest(WWW www)
    {
        yield return www;
      
        // check for errors
        if (www.error == null)
        {
            Debug.Log("WWW Ok!");            
            File.WriteAllBytes(Application.persistentDataPath + "/Info_JSON.json", www.bytes);
            ItemData = JsonMapper.ToObject(File.ReadAllText(Application.persistentDataPath + "/Info_JSON.json"));
            
            Construct();
        }
        else
        {
            Debug.Log("WWW Error: " + www.error);
        }
    }
    


    private void Construct()
    {
        //List<Poi> poisTemp = new List<Poi>();
        //List<Floor> floorsTemp = new List<Floor>();
      /* 
        Center center = new Center(ItemData["data"][0]);       
        foreach (JsonData tempFloor in ItemData["data"]["center"]["floors"])
        {
           
            Floor floorTemp = new Floor(tempFloor);
            poisTemp = new List<Poi>();
            foreach (JsonData tempPoi in tempFloor["pois"])
            {
                Poi poiTemp = new Poi(tempPoi);
                tooltipsTemp = new List<Tooltip>();

                foreach (JsonData tempTooltip in tempPoi["tooltips"])
                {

                    if (tempTooltip != null)
                    {
                        Tooltip tooltipTemp = new Tooltip(tempTooltip);
                        tooltipsTemp.Add(tooltipTemp);
                        //Debug.Log("TYPE: "+ tooltipTemp._id +" type: "+ tooltipTemp.type);
                    }
                    
                }
                poiTemp.tooltips = tooltipsTemp;
                               
                poisTemp.Add(poiTemp);                
            }
                  
            floorTemp.pois = poisTemp;            
            center.floors.Add(floorTemp);
            floorTemp = null;
        }           

        this.center = center;
        end = true;                                                                              //Boolean para determinar que el script termino de leer el JSON y cargarlo   

       saveJSON.ConvertToJson(center);

 */
    }
   
}
