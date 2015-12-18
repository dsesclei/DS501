using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using System.Collections.Generic;

public class Run_Desk : MonoBehaviour {
    
    protected List<MinigameHelper> minigames = new List<MinigameHelper>();
    protected MinigameHelper active_minigame = null;
    protected int active_minigame_index = -1;


    private GameObject[] pool_of_things = null;

    // interfaces
    public Interface_Mouse_Screenspace interface_mouse_screenspace = new Interface_Mouse_Screenspace();
    public Interface_Xbox interface_xbox = new Interface_Xbox();
    public Interface_Leap interface_leap = new Interface_Leap();
    public Interface_Wiimote interface_wiimote = new Interface_Wiimote();

    public virtual void Start()
    {
        // get things pool
        GameObject pool_root = GameObject.Find("ThingsPool");
        pool_of_things = misc.get_children_of( pool_root );
        pool_root.SetActive( false );

        // init all inputs
        LeapMotion.init();
        Wiimote.init();
        Mouse.init();
        Xbox.init();

        // init ScreenspaceCursor
        ScreenspaceCursor.init();
        HeadPose.init();

        MakeMinigames();
    }


    public virtual void MakeMinigames()
    {
        //AddMinigame( new select_three() );
		AddMinigame ( new rotate() );
    }

    public virtual void AddMinigame( Minigame game, Interface inface = null )
    {
        if (inface == null) inface = interface_mouse_screenspace;
        minigames.Add(new MinigameHelper(   game, inface, pool_of_things  ));
    }

    public virtual void Update()
    {
        if (Input.GetKeyDown( KeyCode.R ))
            InputTracking.Recenter();

        if (active_minigame == null || active_minigame.has_ended)
        {
            active_minigame_index++;
			active_minigame_index %= minigames.Count;
            active_minigame = minigames[active_minigame_index];

            active_minigame.go();
        }

    }

}
