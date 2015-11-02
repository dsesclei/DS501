using UnityEngine;
using System.Collections;

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
}
