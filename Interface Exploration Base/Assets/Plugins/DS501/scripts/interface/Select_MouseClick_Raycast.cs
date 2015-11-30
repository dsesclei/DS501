using UnityEngine;
using System.Collections;
using System;

public class Select_MouseClick_Raycast
{
    Software_Cursor cursor = new Software_Cursor();

    Action<GameObject> onSelect = null;//delegate(GameObject obj) {};

	public void   register( Action<GameObject> onSelect )	{	this.onSelect += onSelect;	}
	public void unregister( Action<GameObject> onSelect )	{	this.onSelect -= onSelect;	}

	public Select_MouseClick_Raycast()
	{
        // init the input method; register with relevant listeners
        Mouse.init ();
		Mouse.onDown_Left += delegate() { this.onClick(); };
        Mouse.onMove += this.update_cursor;

        HeadPose.init();
        HeadPose.onMove   += this.update_cursor;
        HeadPose.onRotate += this.update_cursor;
	}

    public void update_cursor()
    {
        cursor.update_position( Mouse.position );
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
