using UnityEngine;
using System.Collections;

public class Record_Selection {
    // This writes info on selection clicks to a CSV, 
    //  in a format we can feed into R

	public string filename;
	public double last_selection_time;
	public int participant_id;

    public string input_name;
    public string interface_name;
    public string task_name;

    public Record_Selection( string filename, 
                             int participant_id,
                             string input_name,
                             string interface_name,
                             string task_name
                           )
	{
		this.filename = filename + ".csv";
		this.participant_id = participant_id;

        this.input_name = input_name;
        this.interface_name = interface_name;
        this.task_name = task_name;
		
		// write header
		CSV.write ( this.filename, 
					"timestamp", "participant", 
                    "input_method", "interface", "task",
                    "target_number",
                    "was_selection_correct",
                    "elapsed", 
                    "distance_click_from_target_center",
                    "distance_target_from_start"
                  );
	}

	public void log(     int target_number,
                        bool correct, 
                     Vector3 target_position, 
                     Vector3 start_position     )
	{
        // get time elapsed since returned to start
		double now = misc.get_timestamp();
		double elapsed = now - last_selection_time;

        // get distance from target center to click
		Vector3 click_position_screenspace = Mouse.position;
        Vector3 target_position_screenspace = 
            Camera.main.WorldToScreenPoint(target_position);
        float distance_click_from_target_center = 
            (click_position_screenspace - target_position_screenspace)
            .magnitude;

        // get distance from start position to target
        Vector3 start_position_screenspace =
            Camera.main.WorldToScreenPoint(start_position);
        float distance_target_from_start =
            ( target_position_screenspace - start_position_screenspace )
            .magnitude;

        // write the acutal log
        CSV.write( filename,
                    now, participant_id,
                    input_name, interface_name, task_name,
                    target_number,
                    correct,
                    elapsed, 
                    distance_click_from_target_center,
                    distance_target_from_start
                  );
        //Debug.Log( "Selection: " + now + " " + correct);
	}

	public void reset_timer( )
	{
		last_selection_time = misc.get_timestamp();
	}

}
