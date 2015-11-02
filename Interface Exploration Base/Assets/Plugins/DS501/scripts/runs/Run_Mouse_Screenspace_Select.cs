using UnityEngine;
using System.Collections;

public class Run_Mouse_Screenspace_Select : MonoBehaviour {

	public GameObject target_group;

	private Task_One_Object_Is_Colored 	task = null;
	private Select_MouseClick_Raycast action = null;

	// Use this for initialization
	void Start () {

		// build our tasks
		task = new Task_One_Object_Is_Colored (target_group);

		// build our interface action logic
		action = new Select_MouseClick_Raycast();
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
}
