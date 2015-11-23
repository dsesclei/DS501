using UnityEngine;
using System.Collections;

public class test_LeapMotion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    LeapMotion.init();
        LeapMotion.onCreate_Right += () => { Debug.Log("Leap: Create R"); };
        //LeapMotion.onMove_RightPalm += () => { 
        //        Debug.Log("Leap: onMove, R Palm: " + Camera.current.WorldToScreenPoint( LeapMotion.pos_right_palm ) ); 
        //    };
        LeapMotion.onPress_RightPalm += () => { Debug.Log("Leap: onPress, R Palm: " + LeapMotion.vel_right_palm.z ); };
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
