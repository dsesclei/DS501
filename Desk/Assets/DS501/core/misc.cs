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
    /*
    public static float Raycast_Find_Collision_Depth(Ray ray, float maxDistance = 100 )
    {

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            return hit.distance;
        }
        else
            return -1;
    }
    */

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

    public static Vector3 ScreenspacePointToPlane( Plane plane, Vector3 screenspace_position )
    {
        // get ray from screenpos
        Ray ray = Camera.main.ScreenPointToRay( screenspace_position );
        
        // cast ray to plane
        float depth;
        plane.Raycast(ray, out depth);
        Vector3 pos_on_ray = ray.GetPoint(depth);

        return pos_on_ray;
    }

    public static List<T> shuffle<T>( List<T> array )
    {
        // inspired by: https://gist.github.com/polysmurf/7393396
        // Loops through array
        for (int i = array.Count - 1; i > 0; i--)
        {
            // Randomize a number between 0 and i (so that the range decreases each time)
            int j = UnityEngine.Random.Range(0, i);

            // Save the value of the current i, otherwise it'll overright when we swap the values
            T temp = array[i];

            // Swap the new and old values
            array[i] = array[j];
            array[j] = temp;
        }

        return array;
    }
}

// Fisher-Yates Shuffle
// https://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net
static class RandomExtensions
{
	public static void Shuffle<T> (this System.Random rng, T[] array)
	{
		int n = array.Length;
		while (n > 1) 
		{
			int k = rng.Next(n--);
			T temp = array[n];
			array[n] = array[k];
			array[k] = temp;
		}
	}
}
