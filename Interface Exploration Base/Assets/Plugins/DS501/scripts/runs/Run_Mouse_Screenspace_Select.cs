using UnityEngine;
using System;
using System.Collections;

public class Run_Mouse_Screenspace_Select : Run
{

	public GameObject target_group;
	public GameObject resetObject;

	private Task_One_Object_Is_Colored task = null;
	private Select_MouseClick_Raycast selectAction = null;

	private Record_Selection selections = null;

	// Use this for initialization
	public void Start ()
	{
		base.Start();

		selections = new Record_Selection( data_file_prefix + "selections"+".csv", participant_id );

		// build our tasks
		task = new Task_One_Object_Is_Colored (target_group, resetObject);

		// build our interface action logic
		selectAction = new Select_MouseClick_Raycast ();
		selectAction.register 	(	this.onSelect	);

        task.start();
    }

    void onSelect(GameObject selected)
    {
        //TODO: we also want to record things here
        if (selected == task.resetObject)
        {
            task.onResetSelected();
            selections.reset_timer();
        }
        else if (selected == task.target_object)
        {
            selections.log(true, task.target_object.transform.position);
            CSV.log(new string[] { "SelectionTask", "Mouse", "Selection correct" });
            task.end();
            task.start();
        }
        else
        {
            selections.log(false, task.target_object.transform.position);
            CSV.log(new string[] { "SelectionTask", "Mouse", "Selection wrong" });
        }
    }
}
