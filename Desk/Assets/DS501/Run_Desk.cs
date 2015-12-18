using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using System.Collections.Generic;

public class Run_Desk : MonoBehaviour {
    
    protected List<MinigameHelper> minigames = new List<MinigameHelper>();
    protected MinigameHelper active_minigame = null;
    protected int active_minigame_index = -1;

    public bool is_showing_instructions = false;


    private GameObject[] pool_of_things = null;

    // interfaces
    public Interface_Mouse_Screenspace interface_mouse_screenspace = new Interface_Mouse_Screenspace();
    public Interface_Xbox interface_xbox = new Interface_Xbox();
    public Interface_Leap interface_leap = new Interface_Leap();
    public Interface_Wiimote interface_wiimote = new Interface_Wiimote();

    public virtual void Start()
    {
        InputTracking.Recenter();

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

        //ScreenspaceCursor.hide();

        MakeMinigames();
    }


    public virtual void MakeMinigames()
    {
        //AddMinigame( new tutorial_select_three() );
        AddMinigame( new rotate(), interface_leap );
        //AddMinigame( new avoid_game() );
        //AddMinigame( new sort_by_color() );
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
            if (active_minigame_index >= minigames.Count)
            {
                //do end
                Debug.Log( "The End." );
            }
            else
            {
                Debug.Log("Starting next minigame ...");
                active_minigame = minigames[active_minigame_index];

                active_minigame.go();
            }
        }

    }

}
