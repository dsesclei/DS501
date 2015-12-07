using UnityEngine;
using System.Collections;
using System;

/*
 * The problem:    Unity doesn't call a script object's Update function unless 
 * 				    it's attatched to a Component in the visual editor.
 *                  Rather a problem for programmatic anything. 
 * This solution:  Build a class that will be attatched to some dummy Component.
 *      		    Other objects register events to this.
 * 
 * Basic inspiration here: http://answers.unity3d.com/questions/20892/update-in-non-component-script.html
 * 
 */

public class OnUpdate : MonoBehaviour {

// the not static stuff
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update() 
	{	
		if (registered_updates != null)
			registered_updates ();	
	}
	//NOTE: Only have one of these per frame, or otherwise we'll need extra logic to 
	//       prevent multiple calls

// The static stuff
	private static Action registered_updates = null;

	public static void register( Action onUpdate_action )
	{
		registered_updates += onUpdate_action;
	}

	public static void unregister( Action onUpdate_action )
	{
		registered_updates -= onUpdate_action;
	}
}
