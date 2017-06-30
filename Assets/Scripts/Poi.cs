using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Poi 
{

	public string _id { get; set; }
	public string floorID { get; set; }
	public int rotationOffset { get; set; }
	public string uri { get; set; }
	public string soundGuide { get; set; }
	//public MapPoint mapPoint = new MapPoint();
	public List<int> position { get; set; }
	public List<Tooltip> tooltips { get; set; }

	/*
    public double[] position = new double[2];
    public double[] mapPos = new double[2];
    public int rotationOffset ;
    public string soundGuide = "";
    public string uri = "";
    public string _id = "";
    public string floorID = "";
    public List<Tooltip1> tooltipList;


    public Poi(JsonData singlePoi)
    {
        this._id = singlePoi["_id"].ToString();
        this.rotationOffset = int.Parse(singlePoi["rotationOffset"].ToString());
        this.uri = singlePoi["uri"].ToString();
        this.soundGuide = singlePoi["soundGuide"].ToString();
        this.floorID= singlePoi["floorID"].ToString();
        //this.mapPos[0] = double.Parse(singleTooltip1["mapPos"].ToString());
        //this.mapPos[1] = double.Parse(singleTooltip1["mapPos"].ToString());
        // tooltipList = this.getTooltip(singlePoi["tooltips"]);
    }
*/
}
