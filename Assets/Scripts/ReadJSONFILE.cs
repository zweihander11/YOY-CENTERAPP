using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class ReadJSONFILE : MonoBehaviour
{
	
	public bool end = false;
	public GameObject sphere3;
	public JsonData ItemData;   
	public Center center;
	private List<Poi> poiListTemp;
	private List<Floor> floorListTemp;
	private List<Tooltip1> tooltipListTemp;


	// Use this for initialization
	void Start()
	{
		if (Application.isMobilePlatform)
		{
			StartCoroutine("GetJsonData");
		}
		else if (Application.isEditor)
		{
			if (File.Exists(Application.streamingAssetsPath + "/Preview_CultureApp.json"))
			{
				// Debug.Log("Exists File JSON");
			}

			center = JsonConvert.DeserializeObject<Center>(File.ReadAllText(Application.streamingAssetsPath + "/Preview_CultureApp.json"));



			end = true;                                                                              //Boolean para determinar que el script termino de leer el JSON y cargarlo   

			//JsonConvert.DeserializedObject (File.ReadAllText(Application.streamingAssetsPath + "/AssetsPreview_CultureApp.json"));
			//PostResponseRegisterTooltip responseRegisterTooltip = JsonConvert.DeserializeObject<PostResponseRegisterTooltip>
			//Construct();
		}

	}



	IEnumerator GetJsonData()
	{
		WWW www = new WWW(Application.streamingAssetsPath + "/Preview_CultureApp.json");
		while (!www.isDone)
		{
			yield return www;
			File.WriteAllBytes(Application.persistentDataPath + "/Preview_CultureApp.json", www.bytes);
		}
		center = JsonConvert.DeserializeObject<Center>(File.ReadAllText(Application.persistentDataPath + "/Preview_CultureApp.json"));
		Debug.Log("Ya lei el .JSON "+center.floors[0]._id);
		end = true;
		//ItemData = JsonMapper.ToObject(File.ReadAllText(Application.persistentDataPath + "/preview_alpha.json"));

	}

	/*

    private void Construct()
    {
        //List<Poi> poiListTemp = new List<Poi>();
        //List<Floor> floorListTemp = new List<Floor>();
		/*
        Center center = new Center(ItemData["data"][0]);

     
        foreach (JsonData tempFloor in ItemData["data"]["center"]["floors"])
        {

            Floor floorTemp = new Floor(tempFloor);
            poiListTemp = new List<Poi>();
            foreach (JsonData tempPoi in tempFloor["pois"])
            {
                Poi poiTemp = new Poi(tempPoi);
                tooltipListTemp = new List<Tooltip1>();

                foreach (JsonData tempTooltip in tempPoi["tooltips"])
                {

                    if (tempTooltip != null)
                    {
                        Tooltip1 tooltipTemp = new Tooltip1(tempTooltip);
                        tooltipListTemp.Add(tooltipTemp);
                        //Debug.Log("TYPE: "+ tooltipTemp._id +" type: "+ tooltipTemp.type);
                    }

                }
                poiTemp.tooltipList = tooltipListTemp;

                poiListTemp.Add(poiTemp);
            }

            floorTemp.poiList = poiListTemp;
            center.floorList.Add(floorTemp);
            floorTemp = null;
        }

        this.center = center;
        end = true;


        this.center = center;
        end = true;                                                                              //Boolean para determinar que el script termino de leer el JSON y cargarlo   

    }    
    */

}
