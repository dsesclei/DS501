using UnityEngine;
using System.Collections;

public class Run_Xbox_Screenspace_Select : Run {
	
	public GameObject target_group;
	
	private Task_One_Object_Is_Colored 	task = null;
	private Select_Xbox_Raycast action = null;
	
	void Start () {

		base.Start();

		task = new Task_One_Object_Is_Colored (target_group);
		
		// build our interface action logic
		action = new Select_Xbox_Raycast ();
		action.register( delegate( GameObject selected ) { this.onSelect( selected ); } );
		
		// run first task
		task.start ();
	}
	
	void onSelect( GameObject selected )
	{
		//TODO: we also want to record things here
		
		if (selected == task.target_object) {
			Debug.Log ("Selection correct!");
			
			// do whatever happens on correct selection
			// (for testing, just reset the task)
			task.end ();
			task.start ();
			
		} else
			Debug.Log ("Selection Wrong.");
		
	}
	
	void OnGUI() {
		if (action != null) {
			// Total hack, but this creates a box that looks like a crosshair
			GUI.Box (new Rect (Xbox.position.x, Xbox.position.y, 10, 10), "");
		}
	}
}