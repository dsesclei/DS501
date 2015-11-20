using UnityEngine;
using System.Collections;

public class Run_Xbox_Screenspace_Select : Base_Run_Select_Mouse
{

    private Select_Xbox_Raycast action = null;

    // Use this for initialization
    public override void Start()
    {

        //NOTE: set experiment_name before calling base.Start()
        experiment_name = "xbox_screenspace_select";
        input_name      = "xbox_controller";
        interface_name  = "mouse_screenspace";   // we're just moving the
                                                 //  mouse cursor
        task_name       = "select";

        base.Start();

        selections = new Record_Selection(data_file_prefix + "selections",
                                            participant_id,
                                            input_name,
                                            interface_name,
                                            task_name
                                         );

        // build our interface action logic
        action = new Select_Xbox_Raycast();
        action.register(delegate(GameObject selected) { this.onSelect(selected); });

        task.start();
    }

	void OnGUI() {
		if (action != null) {
			// Total hack, but this creates a box that looks like a crosshair
			GUI.Box (new Rect (Xbox.position.x, Xbox.position.y, 10, 10), "");
		}
	}
}