using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Floor
{

	public string _id { get; set; }
	public int floorNumber { get; set; }
	public List<Poi> pois { get; set; }
	public string uriMap { get; set; }

	/*
    public string floorNumber="" ;
    public string uriMap="";
    public List<Poi> poiList; 

    public Floor(JsonData singleFloor)
    {                              
        this.floorNumber = singleFloor["floorNumber"].ToString();
        this.uriMap= singleFloor["uriMap"].ToString();
        //poiList = this.getPoi(singleFloor["pois"]);
    }

    */
}
