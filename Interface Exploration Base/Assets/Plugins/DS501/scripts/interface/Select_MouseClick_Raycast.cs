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
        new_pos.z = cursor_rect.position.z;
        //new_pos.x = (new_pos.x * rect.sizeDelta.x) - (0.5f * rect.sizeDelta.x);
        //new_pos.y = (new_pos.y * rect.sizeDelta.y) - (0.5f * rect.sizeDelta.y);
        new_pos.x = (new_pos.x) - (0.5f * rect.sizeDelta.x);
        new_pos.y = (new_pos.y) - (0.5f * rect.sizeDelta.y);
        Debug.Log("New pos: " + new_pos);
        //Debug.Log("RectTransform: " + rect.sizeDelta);
        cursor_rect.position = new_pos;
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
