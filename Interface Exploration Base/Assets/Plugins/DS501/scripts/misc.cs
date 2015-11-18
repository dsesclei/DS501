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

	public static double get_timestamp( )
	{

        double timestamp = (DateTime.UtcNow.Subtract(
                                new DateTime(1970, 1, 1))
                            ).TotalMilliseconds;
        return timestamp;
	}
	public static string get_timestamp_string( )
	{

        double timestamp = (DateTime.UtcNow.Subtract(
                                new DateTime(1970, 1, 1))
                            ).TotalMilliseconds;
        return timestamp.ToString ();
	}

	public static GameObject[] get_children_of( GameObject parent )
	{
		// get array of all children
		// NOTE: this is apparently pretty awkward to do in Unity:
		// http://answers.unity3d.com/questions/594210/get-all-children-gameobjects.html
		// http://forum.unity3d.com/threads/hiow-to-get-children-gameobjects-array.142617/
		// TODO: the second link had a way to add this as a method to GameObject
		GameObject[] children = new GameObject[parent.transform.childCount];
		for ( int i = 0; i < parent.transform.childCount; i++ ) 
		{
			children[i] = parent.transform.GetChild( i ).gameObject;
		}

		return children;
	}
}
