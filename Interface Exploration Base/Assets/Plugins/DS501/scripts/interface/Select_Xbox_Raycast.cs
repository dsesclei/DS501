using UnityEngine;
using System.Collections;
using System;

public class Select_Xbox_Raycast
{
	
	Action<GameObject> onSelect = null;
	private GameObject cursor = GameObject.Find("Cursor");

	public void   register( Action<GameObject> onSelect )	{	this.onSelect += onSelect;	}
	public void unregister( Action<GameObject> onSelect )	{	this.onSelect -= onSelect;	}
	
	public Select_Xbox_Raycast()
	{
		Xbox.init ();
		Xbox.onDown_A += delegate() { this.onClick(); };
	}
	
	public void onClick()
	{
		// Move origin from top left to bottom left
		Vector3 position = new Vector3 (Xbox.position.x, Screen.height - Xbox.position.y);
		Ray ray = Camera.main.ScreenPointToRay (position);
		GameObject selected = misc.Raycast_Select_Object ( ray );
		
		onSelect (selected);
	}
	
	public void onUpdate()
	{
		if (cursor == null) {
			return;
		}
		Vector3 pos = new Vector3(Xbox.position.x, Xbox.position.y, 10);
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
		worldPos.z = -1.5f;
		cursor.transform.position = worldPos;
	}
}