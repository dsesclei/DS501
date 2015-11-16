using UnityEngine;
using System;
using System.Collections;

public class Run_Wiimote_Screenspace_Select : MonoBehaviour
{

	public GameObject target_group;
	public GameObject resetObject;

	private Task_One_Object_Is_Colored 	task = null;
	private Select_MouseClick_Raycast selectAction = null;

	// Use this for initialization
	void Start ()
	{
		// build our tasks
		task = new Task_One_Object_Is_Colored (target_group, resetObject);

		// build our interface action logic
		selectAction = new Select_MouseClick_Raycast ();
		selectAction.register ( delegate( GameObject selected ) { this.onSelect ( selected ); } );

		task.start ();
	}

	void onSelect( GameObject selected )
	{
		//TODO: we also want to record things here
		if (selected == task.resetObject) {
			task.onResetSelected ();
		} else if (task.target_object != null) {
			if (selected == task.target_object) {
				CSV.log (new string[] { "SelectionTask", "Wiimote_Mouse", "Selection correct" });
				task.end ();
				task.start ();
			} else {
				CSV.log (new string[] { "SelectionTask", "Wiimote_Mouse", "Selection wrong" });
			}
		}
	}
}
