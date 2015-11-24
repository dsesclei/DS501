using UnityEngine;
using System.Collections;
using System;

public class Select_MouseClick_Raycast
{

    GameObject cursor = GameObject.Find("Software_Cursor");
    GameObject canvas = GameObject.Find("UI Layer/Canvas");

    Action<GameObject> onSelect = null;//delegate(GameObject obj) {};

	public void   register( Action<GameObject> onSelect )	{	this.onSelect += onSelect;	}
	public void unregister( Action<GameObject> onSelect )	{	this.onSelect -= onSelect;	}

	public Select_MouseClick_Raycast()
	{
        // init the input method; register with relevant listeners
        Mouse.init ();
		Mouse.onDown_Left += delegate() { this.onClick(); };
        Mouse.onMove += this.onMove;
	}

    public void onMove()
    {
        RectTransform rect = canvas.GetComponent<RectTransform>();
        RectTransform cursor_rect = cursor.GetComponent<RectTransform>();
        Vector3 new_pos = Mouse.position;
        int cam_w = Camera.main.pixelWidth;
        int cam_h = Camera.main.pixelHeight;

        float ratio_w = cam_w / rect.sizeDelta.x;
        float ratio_h = cam_h / rect.sizeDelta.y;

        new_pos.z = 98;// cursor_rect.position.z;
        new_pos.x = (new_pos.x * ratio_w) - (0.5f * rect.sizeDelta.x);
        new_pos.y = (new_pos.y * ratio_h) - (0.5f * rect.sizeDelta.y);
        //new_pos.y = -new_pos.y;
        Debug.Log("New pos: " + new_pos);
        Debug.Log("M: " + Mouse.position);
        Debug.Log("RSD: " + rect.sizeDelta);
        //Debug.Log("RectTransform: " + rect.sizeDelta);
        //new_pos = Camera.main.WorldToScreenPoint(new_pos);
        //cursor_rect.position = new_pos;
        cursor.transform.position = new_pos;
        

        //new_pos.x = ((new_pos.x) - (0.5f * rect.sizeDelta.x)) / rect.sizeDelta.x;
        //new_pos.y = ((new_pos.y) - (0.5f * rect.sizeDelta.y)) / rect.sizeDelta.y;
        //cursor_rect.anchoredPosition = new_pos;
        //Debug.Log("New pos: " + new_pos);
    }

	public void onClick()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		GameObject selected = misc.Raycast_Select_Object ( ray );

		// This is an option
		//if (selected == null)	return;

		onSelect (selected);
	}
}
