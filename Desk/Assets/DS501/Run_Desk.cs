using UnityEngine;
using System.Collections;

public class Run_Desk : MonoBehaviour {

	// Use this for initialization
	void Start () {

        // init all inputs
        LeapMotion.init();
        Wiimote.init();
        Mouse.init();
        Xbox.init();

	}
	
	// Update is called once per frame
	void Update () {
	    


	}

}
