using UnityEngine;
using System.Collections;

public class Run_Desk : MonoBehaviour {

	void Start () {

        // init all inputs
        LeapMotion.init();
        Wiimote.init();
        Mouse.init();
        Xbox.init();

        // init ScreenspaceCursor
        HeadPose.init();
        HeadPose.onMove     += () => { ScreenspaceCursor.update_cursor(Mouse.position); };
        HeadPose.onRotate   += () => { ScreenspaceCursor.update_cursor(Mouse.position); };

        // TODO: move this to interface
        Mouse.onMove += () => { ScreenspaceCursor.update_cursor(Mouse.position); };
    }
	

	void Update () {
	    


	}

}
