﻿using UnityEngine;
using System.Collections;
using System;

public class Select_MouseClick_Raycast
{

	Action<GameObject> onSelect = null;//delegate(GameObject obj) {};
	GameObject cursor = GameObject.Find("Cursor");
	public void   register( Action<GameObject> onSelect )	{	this.onSelect += onSelect;	}
	public void unregister( Action<GameObject> onSelect )	{	this.onSelect -= onSelect;	}

	public Select_MouseClick_Raycast()
	{
		// init the input method; register with relevant listeners
		Mouse.init ();
		Mouse.onDown_Left += delegate() { this.onClick(); };
		Mouse.onMove += delegate() { this.onMove(); };
	}

	public void onClick()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		GameObject selected = misc.Raycast_Select_Object ( ray );

		// This is an option
		//if (selected == null)	return;

		onSelect (selected);
	}

	public void onMove()
	{
		Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
		worldPos.z = -1.5f;
		cursor.transform.position = worldPos;
	}
}
