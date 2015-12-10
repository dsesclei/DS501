using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Run_Desk : MonoBehaviour {

    // interfaces
    Interface_Mouse_Screenspace interface_mouse_screenspace = new Interface_Mouse_Screenspace();

    // minigames
    List<MinigameHelper> minigames = new List<MinigameHelper>();
    MinigameHelper active_minigame = null;
    int active_minigame_index = -1;

    void Start () {

        // init all inputs
        LeapMotion.init();
        Wiimote.init();
        Mouse.init();
        Xbox.init();

        // init ScreenspaceCursor
        ScreenspaceCursor.init();
        HeadPose.init();
        HeadPose.onMove     += () => { ScreenspaceCursor.update_position(Mouse.position); };
        HeadPose.onRotate   += () => { ScreenspaceCursor.update_position(Mouse.position); };

        //build minigames
        minigames.Add( new MinigameHelper( new select_three(), interface_mouse_screenspace ) );

    }
	

	void Update () {
	    
        if( active_minigame == null || active_minigame.has_ended )
        {
            active_minigame_index++;
            active_minigame = minigames[active_minigame_index];

            active_minigame.go();
        }

	}

}
