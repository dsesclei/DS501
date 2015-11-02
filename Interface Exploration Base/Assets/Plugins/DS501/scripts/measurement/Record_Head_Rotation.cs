using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class Record_Head_Rotation {

	// Getting values as per here:
	// http://docs.unity3d.com/Manual/VROverview.html

	public static void start()
	{
		OnUpdate.register( Record_Head_Rotation.record );
	}
	public static void stop()
	{		
		OnUpdate.unregister( Record_Head_Rotation.record );
	}

	public static void record()
	{
		Debug.Log ("VR Rot: " + InputTracking.GetLocalRotation (0));
		Debug.Log ("VR Pos: " + InputTracking.GetLocalPosition (0));
		//CSV.write_data ("");
	}

}
