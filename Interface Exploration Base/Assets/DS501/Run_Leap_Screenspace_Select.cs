using UnityEngine;
using System;
using System.Collections;

public class Run_Leap_Screenspace_Select : Base_Run_Select_Mouse
{

    protected Select_Leap_RightPalm_Screenspace_Cursor selectAction = null;

	// Use this for initialization
	public override void Start ()
	{
        //NOTE: set experiment_name before calling base.Start()
        experiment_name = "leap_screenspace_select";
        input_name      = "leap";
        interface_name  = "cursor_screenspace";   // we're just moving the
                                                  //  mouse cursor
        task_name       = "select";

        base.Start();
        
        selections = new Record_Selection(  data_file_prefix + "selections",
                                            participant_id,
                                            input_name,
                                            interface_name,
                                            task_name
                                         );

        // build our interface action logic
        selectAction = new Select_Leap_RightPalm_Screenspace_Cursor();
        selectAction.register(this.onSelect);

        task.start();
    }
    
}
