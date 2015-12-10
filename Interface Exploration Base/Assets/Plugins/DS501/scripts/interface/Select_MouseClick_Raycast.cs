using UnityEngine;
using System.Collections;
using System;

public class Select_MouseClick_Raycast
{
    Software_Cursor cursor = new Software_Cursor();

    Action<GameObject> onSelect = null;//delegate(GameObject obj) {};

	public void   register( Action<GameObject> onSelect )	{	this.onSelect += onSelect;	}
	public void unregister( Action<GameObject> onSelect )	{	this.onSelect -= onSelect;	}
	private GameObject cursor = GameObject.Find("Cursor");

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

	public void onUpdate()
	{
		if (cursor == null) {
			return;
		}
		Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
		worldPos.z = -1.5f;
		cursor.transform.position = worldPos;
	}
}
