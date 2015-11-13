using UnityEngine;
using System.Collections;
using System;

public class Select_MouseClick_Raycast
{

	Action<GameObject> onSelect = null;//delegate(GameObject obj) {};

	public void   register( Action<GameObject> onSelect )	{	this.onSelect += onSelect;	}
	public void unregister( Action<GameObject> onSelect )	{	this.onSelect -= onSelect;	}

	public Select_MouseClick_Raycast()
	{
		// init the input method; register with relevant listeners
		Mouse.init ();
		Mouse.onDown_Left += delegate() { this.onClick(); };
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
