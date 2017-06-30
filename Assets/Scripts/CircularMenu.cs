using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CircularMenu : MonoBehaviour {
    public List<MenuButton> buttons = new List<MenuButton>();
    private Vector2 Mouseposition ;
    private Vector2 fromVector2M = new Vector2(0.5f, 1.0f);
    private Vector2 centercircle = new Vector2(0.5f, 0.5f);
    private Vector2 toVector2M;

    public int menuItems;
    public int CurMenuItem;
    private int OldMenuItem;




	// Use this for initialization
	void Start () {
        menuItems = buttons.Count;
        foreach (MenuButton button in buttons)
        {
            button.sceneimage.color = button.NormalColor;
        }
        CurMenuItem = 0;
        OldMenuItem = 0; 
	}
	
	// Update is called once per frame
	void Update () {
        GetCurrentMenuItem();
        if (Input.GetButtonDown("Fire1"));
        ButtonAction();
        {

        }
	}

    public void GetCurrentMenuItem()
    {
        Mouseposition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        toVector2M = new Vector2(Mouseposition.x/Screen.width, Mouseposition.y/Screen.height);
        float angle = (Mathf.Atan2(fromVector2M.y - centercircle.y, fromVector2M.x - centercircle.x) - Mathf.Atan2(toVector2M.y - centercircle.y, toVector2M.x - centercircle.x)) * Mathf.Rad2Deg;

        if (angle<0)
        {
            angle += 360;
            CurMenuItem = ((int)angle / (360/menuItems));

        }
        if (CurMenuItem != OldMenuItem)
        {
            buttons[OldMenuItem].sceneimage.color= buttons[OldMenuItem].NormalColor;
            OldMenuItem = CurMenuItem;
            buttons[CurMenuItem].sceneimage.color = buttons[CurMenuItem].HighlightedColor;

        }
    }



public void ButtonAction()
    {
        buttons[CurMenuItem].sceneimage.color = buttons[CurMenuItem].PressedColor;
        if (CurMenuItem==0)
        {
            print("You have pressed the first button");
        }
    } 


[System.Serializable]
public class MenuButton
    {
        public string name;
        public Image sceneimage;
        public Color NormalColor = Color.white;
        public Color HighlightedColor = Color.grey;
        public Color PressedColor = Color.gray;
    }




}
