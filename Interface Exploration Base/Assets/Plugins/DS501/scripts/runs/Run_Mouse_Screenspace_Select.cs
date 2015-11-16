using UnityEngine;
using System;
using System.Collections;

public class Run_Mouse_Screenspace_Select : Run
{

	public GameObject target_group;

	private Task_One_Object_Is_Colored task = null;
	private Select_MouseClick_Raycast selectAction = null;
	private Move_Mouse moveAction = null;
	private float resetBoxHeight = Screen.height / 10;
	private bool waitingForReset = true;

	private Record_Selection selections = null;

	// Use this for initialization
	public void Start ()
	{
		base.Start();

		selections = new Record_Selection( data_file_prefix + "selections", participant_id );

		// build our tasks
		task = new Task_One_Object_Is_Colored (target_group);

		// build our interface action logic
		selectAction = new Select_MouseClick_Raycast ();
		selectAction.register 	(	this.onSelect	);

		moveAction = new Move_Mouse ();
		moveAction.register 	(	this.onMove		);
	}

	public void onSelect( GameObject selected )
	{
		//TODO: we also want to record things here
		if (selected == task.target_object) {
			selections.log( true, task.target_object.transform.position );
			//CSV.log (new string[] { "SelectionTask", "Mouse", "Selection correct" });
			task.deselectAll ();
			waitingForReset = true;
		} else {
			selections.log( false, task.target_object.transform.position );
			//CSV.log (new string[] { "SelectionTask", "Mouse", "Selection wrong" });
		}
	}

	public void onMove( Vector3 position )
	{
		if (waitingForReset && position.y < resetBoxHeight) {
			waitingForReset = false;
			if (task.isRunning) {
				task.end ();
			}
			task.start ();
			selections.reset_timer();
		}
	}

	public void OnGUI()
	{
		GUI.Box (new Rect (0, Screen.height - resetBoxHeight, Screen.width, resetBoxHeight), "");
	}
}
