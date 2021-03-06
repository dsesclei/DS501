﻿using UnityEngine;
using System;
using System.Collections;

public class Base_Run_Select_Mouse : Run
{

    public GameObject targetObject;
    public GameObject resetObject;
	public GameObject cursorObject;

    protected Task_One_Object_Is_Colored task = null;

    protected Record_Selection selections = null;

    protected int target_number = 0;

    // Use this for initialization
    public override void Start()
    {
        //NOTE: set experiment_name before calling base.Start()
        base.Start();

		Cursor.visible = false;

        // build our tasks
        task = new Task_One_Object_Is_Colored(targetObject, resetObject);
    }

    public virtual void onSelect(GameObject selected)
    {
        //TODO: we also want to record things here
        if (selected == task.resetObject)
        {
            task.onResetSelected();
            selections.reset_timer();
            target_number++;
        }
        else if (selected == task.targetObject)
        {
            Vector3 target_position = task.targetObject.transform.position;
            Vector3 start_position  = resetObject.transform.position;

            selections.log(target_number, true, target_position, start_position);
			task.onTargetSelected();
			if (task.isOver)
			{
				task.end ();
			}
        }
        else
        {
            Vector3 target_position = task.targetObject.transform.position;
            Vector3 start_position  = resetObject.transform.position;

            selections.log(target_number, false, target_position, start_position);
        }
    }
}
