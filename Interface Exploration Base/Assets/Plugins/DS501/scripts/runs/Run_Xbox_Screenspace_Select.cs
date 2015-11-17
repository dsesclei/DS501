using UnityEngine;
using System.Collections;

public class Run_Xbox_Screenspace_Select : Run {
	
	public GameObject target_group;
	public GameObject resetObject;
	
	private Task_One_Object_Is_Colored 	task = null;
	private Select_Xbox_Raycast action = null;
	
	void Start () {

		base.Start();
		task = new Task_One_Object_Is_Colored (target_group, resetObject);
		
		// build our interface action logic
		action = new Select_Xbox_Raycast ();
		action.register( delegate( GameObject selected ) { this.onSelect( selected ); } );
		
		// run first task
		task.start ();
	}
	
	void onSelect( GameObject selected )
	{
		if (selected == task.resetObject) {
			task.onResetSelected ();
		} else if (task.target_object != null) {
			if (selected == task.target_object) {
				CSV.log (new string[] { "SelectionTask", "Mouse", "Selection correct" });
				task.end ();
				task.start ();
			} else {
				CSV.log (new string[] { "SelectionTask", "Mouse", "Selection wrong" });
			}
		}
	}

	void OnGUI() {
		if (action != null) {
			// Total hack, but this creates a box that looks like a crosshair
			GUI.Box (new Rect (Xbox.position.x, Xbox.position.y, 10, 10), "");
		}
	}
}