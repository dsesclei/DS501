using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class Record_Head_Movement {

    // Getting values as per here:
    // http://docs.unity3d.com/Manual/VROverview.html

    //TODO: good chance that with writing every frame, 
    //       we just want to leave the file open

	public string filename;

    private Quaternion last_rot;
    private Vector3 last_pos;

	public Record_Head_Movement( string filename )
	{
		this.filename = filename + ".csv";

		// write header
		CSV.write ( this.filename,	
                      "timestamp", 
                      "rot_x", "rot_y", "rot_z", "rot_w",
                      "pos_x", "pos_y", "pos_z"
                  );
	}

	public void start()
	{

        last_rot = InputTracking.GetLocalRotation(0);
        last_pos = InputTracking.GetLocalPosition(0);
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
		string timestamp = misc.get_timestamp_string();

        // only write on change (mostly so we don't make big files during
        //  debug)
        if (last_rot == rot && last_pos == pos)
            return;

		CSV.write ( filename, 	
                    timestamp, 
                    rot.x, rot.y, rot.z, rot.w,
                    pos.x, pos.y, pos.z	  
                  );
	}

}
