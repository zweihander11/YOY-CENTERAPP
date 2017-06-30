using System;
using System.Collections.Generic;
using LitJson;
using UnityEngine;

[Serializable]
public class Center
{

	public string nav_icon { get; set; }
	public string firstPoiID { get; set; }
	public string firstFloorID { get; set; }
	public int firstPoiRotation { get; set; }
	public string _id { get; set; }
	//public List<Floor> floors { get; set; }
	public List<Floor> floors { get; set; }


	/*
    public string _id = "";
    public string nav_icon = "";
    public string firstPoiID = "";
    public string firstFloorID = "";
    public int firstPoiRotation;
    public List<Floor> floorList = new List<Floor>();   
    public Center(JsonData data)
    {
        //this._id = data["_id"].ToString();     
       this.nav_icon = data["nav_icon"].ToString();
        this.firstPoiID = data["firstPoiID"].ToString();
        this.firstFloorID = data["firstFloorID"].ToString();
        this.firstPoiRotation = int.Parse(data["firstPoiRotation"].ToString());        
        //tooltipList = this.getTooltip(singlePoi["tooltips"]);
        //floorList = this.getFloor(data["floors"]);      

    }      
*/
}