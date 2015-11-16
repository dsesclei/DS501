using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// misc: a place for things when we don't know their place

public class misc 
{
	public static GameObject Raycast_Select_Object( Ray ray )
	{

		RaycastHit hit;
		float maxDistance = 100; // TODO: this may need adjusting

		if ( Physics.Raycast ( ray, out hit, maxDistance ) )
		{
			return hit.collider.gameObject;
		}
		else
			return null;
	}

	public static string get_timestamp( )
	{
		Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		return unixTimestamp.ToString ();
	}
	public static int get_timestamp_int( )
	{
		Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		return unixTimestamp;
	}
	public static string get_timestamp_string( )
	{
		Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		return unixTimestamp.ToString ();
	}
}
