using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Run_Desk : MonoBehaviour {

    // interfaces
    Interface_Mouse_Screenspace interface_mouse_screenspace = new Interface_Mouse_Screenspace();

    // minigames
    List<Minigame> minigames = new List<Minigame>();
    active_minigame = null;

    void Start () {

        // init all inputs
        LeapMotion.init();
        Wiimote.init();
        Mouse.init();
        Xbox.init();

        // init ScreenspaceCursor
        HeadPose.init();
        HeadPose.onMove     += () => { ScreenspaceCursor.update_position(Mouse.position); };
        HeadPose.onRotate   += () => { ScreenspaceCursor.update_position(Mouse.position); };

        //build minigames


    }
	

	void Update () {
	    


	}

}
