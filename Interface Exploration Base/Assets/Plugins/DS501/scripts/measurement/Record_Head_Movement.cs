using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class Record_Head_Movement {

	// Getting values as per here:
	// http://docs.unity3d.com/Manual/VROverview.html

	//TODO: good chance that with writing every frame, we just want to leave the file open

	public string filename;

	public Record_Head_Movement( string filename )
	{
		this.filename = filename;

		// write header
		CSV.write ( filename,	"timestamp", "rotation", "position"	  );
	}

	public void start()
	{
		OnUpdate.register( this.record );
	}
	public void stop()
	{		
		OnUpdate.unregister( this.record );
	}

	public void record()
	{
		//Debug.Log ("VR Rot: " + InputTracking.GetLocalRotation (0));
		//Debug.Log ("VR Pos: " + InputTracking.GetLocalPosition (0));
		Quaternion rot = InputTracking.GetLocalRotation (0);
		   Vector3 pos = InputTracking.GetLocalPosition (0);
		string timestamp = misc.get_timestamp();

		CSV.write ( filename, 	timestamp, rot, pos	  );
	}

}
