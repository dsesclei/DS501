using UnityEngine;
using System.Collections;

public class Record_Mouse {

	//TODO: good chance we just want to leave the file open

	public string filename;

	public Record_Mouse( string filename )
	{
		this.filename = filename;
		
		// write header
		CSV.write ( filename, 
					"timestamp", "position"
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
		string timestamp = misc.get_timestamp();

		CSV.write ( filename, 
					timestamp, position
				  );
	}

}
