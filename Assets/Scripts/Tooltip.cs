using LitJson;
using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Tooltip 
{
	public string floorID { get; set; }
	public string _id { get; set; }
	public string poiID { get; set; }
	public string type { get; set; }
	public string text { get; set; }
	public float width { get; set; }
	public float height { get; set; }
	public List<int> position { get; set; }
	public List<string> category { get; set; }
	public string soundGuide { get; set; }
	public string attribution { get; set; }
	public string attributionUri { get; set; }
	public string title { get; set; }
	public string source { get; set; }
	public string sourcePano { get; set; }
	public string sourceVideo { get; set; }
	public string sourceUriLeft { get; set; }
	public string sourceUriRight { get; set; }
	public string linkedPoiID { get; set; }


	/*
    public string text;
    public float[] position;
    public string linkedPhotoId;
    public string type;
    public float width;
    public float height;
    public string source;
    public string uri;
    //public Vector3 position;
    public string attribution;
    public string attributionUri;
    public string attributionUriRight;
    public string attributionUriLeft;
    public string attributionUriAudio;
*/



}