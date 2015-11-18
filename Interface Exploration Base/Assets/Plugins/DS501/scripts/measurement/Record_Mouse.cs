using UnityEngine;
using System.Collections;

public class Record_Mouse {

	//TODO: good chance we just want to leave the file open

	public string filename;

	public Record_Mouse( string filename )
	{
		this.filename = filename + ".csv";
		
		// write header
		CSV.write ( this.filename, 
					"timestamp", 
                    "position_x", "position_y", "position_z"
                  );
	}

	public void start()
	{
		Mouse.onMove += this.record;
	}
	public void stop()
	{		
		Mouse.onMove -= this.record;
	}

	public void record()
	{
		Vector3 position = Mouse.position;
		string timestamp = misc.get_timestamp_string();

		CSV.write ( filename, 
					timestamp, 
                    position.x, position.y, position.z
				  );
	}

}
