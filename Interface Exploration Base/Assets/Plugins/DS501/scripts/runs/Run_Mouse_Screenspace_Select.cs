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
		string timestamp = DateTime.Now.ToString ("MM/dd/yyyy h:mm tt");
		if (selected == task.target_object) {
			CSV.write_data ("log.txt", new string[] { timestamp, "SelectionTask", "Selection correct" });
			Debug.Log ("Selection correct!");
			task.deselectAll ();
			waitingForReset = true;
		} else {
			CSV.write_data ("log.txt", new string[] { timestamp, "SelectionTask", "Selection wrong" });
			Debug.Log ("Selection Wrong.");
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
