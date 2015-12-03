using UnityEngine;
using System.Collections;
using System;

public class Drag_Mouse {
	
	Action<Vector3> onDrag;

	private bool isDragging = false;
	private Vector3 lastMousePosition;
	
	public void   register( Action<Vector3> onDrag ) { this.onDrag += onDrag; }
	public void unregister( Action<Vector3> onDrag ) { this.onDrag -= onDrag; }
	
	public Drag_Mouse()
	{
		Mouse.init ();
		Mouse.onMove += delegate() {
			if (isDragging) {
				onMouseDrag();
			}
		};
		Mouse.onDown_Left += delegate() { isDragging = true; };
		Mouse.onUp_Left += delegate() { isDragging = false; };
	}
	
	public void onMouseDrag()
	{
		Vector3 dragDelta = Mouse.position - Mouse.last_position;
		onDrag(dragDelta);
	}
	
}