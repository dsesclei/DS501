using UnityEngine;
using System.Collections;

public class Record_Selection {


	public string filename;
	public int last_selection_time;
	public int participant_id;

	public Record_Selection( string filename, int participant_id )
	{
		this.filename = filename;
		this.participant_id = participant_id;
		
		// write header
		CSV.write ( filename, 
					"timestamp", "participant", "was_selection_correct", "elapsed", "position", "target_position"
				  );
	}

	public void log( bool correct, Vector3 target_position )
	{
		int now = misc.get_timestamp_int();
		int elapsed = now - last_selection_time;
		Vector3 position = Mouse.position;


		CSV.write ( filename, 
					now, participant_id, correct, elapsed, position, target_position
				  );
	}

	public void reset_timer( )
	{
		last_selection_time = misc.get_timestamp_int();
	}

}
