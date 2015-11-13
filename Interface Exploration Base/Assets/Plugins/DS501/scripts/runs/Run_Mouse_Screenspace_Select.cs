using UnityEngine;
using System;
using System.Collections;

public class Run_Mouse_Screenspace_Select : MonoBehaviour
{

	public GameObject target_group;

	private Task_One_Object_Is_Colored 	task = null;
	private Select_MouseClick_Raycast selectAction = null;
	private Move_Mouse moveAction = null;
	private float resetBoxHeight = Screen.height / 10;
	private bool waitingForReset = true;

	// Use this for initialization
	void Start ()
	{

		// build our tasks
		task = new Task_One_Object_Is_Colored (target_group);

		// build our interface action logic
		selectAction = new Select_MouseClick_Raycast ();
		selectAction.register ( delegate( GameObject selected ) { this.onSelect ( selected ); } );

		moveAction = new Move_Mouse ();
		moveAction.register ( delegate( Vector3 position ) { this.onMove( position ); } );
	}

	void onSelect( GameObject selected )
	{
		//TODO: we also want to record things here
		if (selected == task.target_object) {
			CSV.log (new string[] { "SelectionTask", "Mouse", "Selection correct" });
			task.deselectAll ();
			waitingForReset = true;
		} else {
			CSV.log (new string[] { "SelectionTask", "Mouse", "Selection wrong" });
		}
	}

	void onMove( Vector3 position )
	{
		if (waitingForReset && position.y < resetBoxHeight) {
			waitingForReset = false;
			if (task.isRunning) {
				task.end ();
			}
			task.start ();
		}
	}

	void OnGUI()
	{
		GUI.Box (new Rect (0, Screen.height - resetBoxHeight, Screen.width, resetBoxHeight), "");
	}
}
