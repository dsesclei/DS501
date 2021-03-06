﻿using UnityEngine;
using System.Collections;
using System;

public class Mouse : MonoBehaviour
{
	//public static GameObject root = null;

	public static bool  left_down            = false,
                        right_down           = false,
                        last_left_down       = false,
		 				last_right_down      = false;
	public static Vector3 	position 		 = new Vector3(),
							last_position 	 = new Vector3(),
							velocity 		 = new Vector3();

	private static bool has_been_initialized = false;

	// must call init in main run file
	public static void init ( )//GameObject root ) 
	{
        //Debug.Log("Mouse Init called.");
		//Mouse.root = root;

		//TODO: this check is maybe not robust to parallel calls?
		if( has_been_initialized )
			return;

		has_been_initialized = true;
		OnUpdate.register ( delegate() { Mouse.update(); } );
	}


	public static void update () 
	{
		position = Input.mousePosition;

		// check for events
		if (Input.GetMouseButtonDown (0)) 		  Mouse.onDown_Left  ();
		if (Input.GetMouseButtonDown (1)) 		  Mouse.onDown_Right ();
		if (Input.GetMouseButtonUp   (0)) 		  Mouse.onUp_Left  ();
		if (Input.GetMouseButtonUp   (1)) 		  Mouse.onUp_Right ();
		if (position != last_position) 	  		  Mouse.onMove ();

		// update stored state
		velocity = Input.mousePosition - last_position;
        if (Input.GetMouseButton(0))    left_down = true;
        else                            left_down = false;
		if (Input.GetMouseButton (1))	right_down = true;
        else                            right_down = false;

        last_position = Input.mousePosition;
        last_left_down = left_down;
        last_right_down = right_down;
    }

	public static Action onDown_Left  = () => { };
	public static Action onDown_Right = () => { };
	public static Action onUp_Left    = () => { };
	public static Action onUp_Right   = () => { };
	public static Action onMove 	  = () => { };

}
