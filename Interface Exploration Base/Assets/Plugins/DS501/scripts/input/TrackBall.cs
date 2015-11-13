using UnityEngine;
using System.Collections;
using System;

public class TrackBall : MonoBehaviour
{

	public static bool     last_left_down    = false;
	public static bool     last_right_down   = false;
	public static Vector3  position          = new Vector3();
	public static Vector3  last_position     = new Vector3();
	public static Vector3  velocity          = new Vector3();
	public static Vector3  last_velocity     = new Vector3();
	public static Vector3  acceleration      = new Vector3();
	public static Vector3  last_acceleration = new Vector3();

	private static bool has_been_initialized = false;

	// must call init in main run file
	public static void init ( )//GameObject root ) 
	{
		if( has_been_initialized )
			return;

		has_been_initialized = true;
		OnUpdate.register ( delegate() { TrackBall.update(); } );
	}


	public static void update () 
	{
		//FIXME update() probably isn't occuring at a constant interval
		//and velocity / acceleration are both off by a bit.
		if (position != null)
			last_position = position;
		position = Input.mousePosition;
		if (velocity != null)
			last_velocity = velocity;
		velocity = position - last_position;
		if (acceleration != null)
			last_acceleration = acceleration;
		if (last_velocity != null)
			acceleration = Math.abs(velocity - last_velocity);
			

		// check for events
		if (Input.GetMouseButtonDown (0))   Mouse.onDown_Left  ();
		if (Input.GetMouseButtonDown (1))   Mouse.onDown_Right ();
		if (Input.GetMouseButtonUp   (0))   Mouse.onUp_Left  ();
		if (Input.GetMouseButtonUp   (1))   Mouse.onUp_Right ();
		if (position != last_position) 	    Mouse.onMove ();

		// update stored state
		if (Input.GetMouseButton (0))	last_left_down  = true;
		if (Input.GetMouseButton (1))	last_right_down = true;
	}

	public static Action onDown_Left  = () => { };
	public static Action onDown_Right = () => { };
	public static Action onUp_Left    = () => { };
	public static Action onUp_Right   = () => { };
	public static Action onMove 	  = () => { };

}
