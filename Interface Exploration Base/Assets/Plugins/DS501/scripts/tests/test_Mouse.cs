using UnityEngine;
using System.Collections;

public class test_Mouse : MonoBehaviour {
	
	//public GameObject target_group;
	
	// Use this for initialization
	void Start () {

		Mouse.init ();

		Mouse.onUp_Left   += 
		  () => { Debug.Log ("Mouse: Left Up, Velocity: "  + Mouse.velocity); };
		Mouse.onUp_Right  += 
		  () => { Debug.Log ("Mouse: right Up, Velocity: " + Mouse.velocity); };
		Mouse.onDown_Left += 
		  () => { Debug.Log ("Mouse: Left Down, Position: "  + Mouse.position); };
		Mouse.onDown_Left += 
		  () => { Debug.Log ("Mouse: Left Down, Position: "  + Mouse.position); };
		Mouse.onMove      += 
		  () => { Debug.Log ("Mouse move, vel:" + Mouse.velocity); };
	}
}
