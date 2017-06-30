using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Tooltip1  {
    public string floorID = "";
    public string type="";
    public string _id="";
    public string text = "";
    public double[] position = new double[2];
   
    public string soundGuide = "";
    public string linkedPoiID = "";
    public string linkedFloorID = "";
    public float width ;
    public float height ;
    public string source = "";
    public string attribution = "";
    public string attributionUri = "";
    public string sourceUriLeft = "";
    public string sourceUriRight = "";
    public string sourceVideo = "";
    public string sourcePano = "";
    public string title = "";

    public Tooltip1 (JsonData singleTooltip1)
    {
        //this._id = singleTooltip1["_id"].ToString();
        this.type = singleTooltip1["type"].ToString();

        if (singleTooltip1["text"] != null)
        {
            this.text = singleTooltip1["text"].ToString();
        }
        
        this.position[0]= double.Parse(singleTooltip1["position"][0].ToString());
        this.position[1] = double.Parse(singleTooltip1["position"][1].ToString());
        this.soundGuide = singleTooltip1["soundGuide"].ToString();
        this.attribution = singleTooltip1["attribution"].ToString();
        this.attributionUri = singleTooltip1["attributionUri"].ToString();
        this.width = float.Parse(singleTooltip1["width"].ToString());
        this.height = float.Parse(singleTooltip1["height"].ToString());
       

        if (type == "GATE" )
        {
            //this.floorID = singleTooltip1["floorID"].ToString();
            this.linkedPoiID = singleTooltip1["linkedPoiID"].ToString();
            this.linkedFloorID = singleTooltip1["linkedFloorID"].ToString();
        }

        if (type == "PANELIMAGE")
        {
            this.source = singleTooltip1["source"].ToString();

        }

        if (type == "TEXTBLOCK")
        {
            this.title = singleTooltip1["title"].ToString();

        }

        if (type == "STEREOSCOPIC")
        {
            this.sourceUriLeft = singleTooltip1["sourceUriLeft"].ToString();
            this.sourceUriRight = singleTooltip1["sourceUriRight"].ToString();

        }

        if (type == "VIDEO")
        {
            this.sourceVideo = singleTooltip1["sourceVideo"].ToString();

        }

        if ((singleTooltip1["type"].ToString()) == "PANO")
        {
            this.sourcePano = singleTooltip1["sourcePano"].ToString();
        }
    }
    
}


