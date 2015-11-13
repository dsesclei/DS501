using UnityEngine;
using System.Collections;
using System;

public class Move_Mouse {
	
	Action<Vector3> onMove = null;
	
	public void   register( Action<Vector3> onMove ) { this.onMove += onMove; }
	public void unregister( Action<Vector3> onMove ) { this.onMove -= onMove; }
	
	public Move_Mouse()
	{
		Mouse.init ();
		Mouse.onMove += delegate() { this.onMouseMove(); };
	}
	
	public void onMouseMove()
	{
		onMove (Input.mousePosition);
	}
	
}
