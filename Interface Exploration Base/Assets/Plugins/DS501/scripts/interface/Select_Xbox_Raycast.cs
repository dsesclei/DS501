using UnityEngine;
using System.Collections;
using System;

public class Select_Xbox_Raycast
{
	
	Action<GameObject> onSelect = null;
	
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
}