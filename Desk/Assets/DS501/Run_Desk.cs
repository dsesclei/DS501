using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

public class Run_Desk : MonoBehaviour {
    
    protected List<MinigameHelper> minigames = new List<MinigameHelper>();
    protected MinigameHelper active_minigame = null;
    protected int active_minigame_index = -1;

    public bool is_showing_instructions = false;
    public bool is_showing_results = false;
    
    public bool is_ended = false;

    public bool should_scene_advance = false;
    Timer scene_timer = new Timer(2000);

    private GameObject[] pool_of_things = null;

    private GameObject failure_text;
    private GameObject success_text;
    private GameObject instructions_text;

    // interfaces
    public Interface_Mouse_Screenspace interface_mouse_screenspace = new Interface_Mouse_Screenspace();
    public Interface_Xbox interface_xbox = new Interface_Xbox();
    public Interface_Leap interface_leap = new Interface_Leap();
    public Interface_Wiimote interface_wiimote = new Interface_Wiimote();

    public virtual void Start()
    {
        // this is used in update
        scene_timer.Elapsed += (object sender, ElapsedEventArgs e) =>
        {
            //Debug.Log("Scene Timer Elapsed!");
            should_scene_advance = true;
            scene_timer.Stop();
        };

        InputTracking.Recenter();

        // get things pool, hide some stuff
        GameObject pool_root = GameObject.Find("ThingsPool");
        pool_of_things = misc.get_children_of( pool_root );
        pool_root.SetActive( false );


        failure_text = GameObject.Find("Text: NO");
        success_text = GameObject.Find("Text: YES");
        instructions_text = GameObject.Find("Text: Instructions");
        success_text.SetActive(false);
        failure_text.SetActive(false);
        instructions_text.SetActive(false);

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
        AddMinigame( new rotate() );
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

        //TODO: reset button?

        if (is_ended) return;

        if (active_minigame == null || active_minigame.has_ended)
        // time to show results, process last game, and advance
        {
            //process last game, if exists
            if (active_minigame != null)
            {
                //show results
                if (!is_showing_results)
                {
                    //TODO: logging

                    if (active_minigame.success) success_text.SetActive(true);
                    else failure_text.SetActive(true);
                    is_showing_results = true;

                    //scene_timer.Interval = 1000;
                    scene_timer.Start();
                    // this sets should_scene_advance when it expires

                }
            }

            if ( is_showing_results && should_scene_advance
                 ||   active_minigame == null )
            {
                //advance from show results
                success_text.SetActive(false);
                failure_text.SetActive(false);

                should_scene_advance = false;
                is_showing_results = false;

                if (active_minigame_index + 1 >= minigames.Count)
                // end if we're out of games
                {
                    //do end
                    Debug.Log("  The End.");
                    is_ended = true;
                    instructions_text.GetComponent<TextMesh>().text = "The\nEnd";
                    instructions_text.SetActive(true);
                }
                else// if (!is_showing_instructions)
                // show instructions for next game
                {
                    // get next game
                    active_minigame_index++;
                    Debug.Log("Starting next minigame ... " + active_minigame_index + " of " + minigames.Count);

                    active_minigame = minigames[active_minigame_index];
                    //active_minigame.onButton += onPress_set_should_scene_advance;

                    active_minigame.prestart();

                    instructions_text.GetComponent<TextMesh>().text = active_minigame.instructions;
                    instructions_text.SetActive(true);

                    is_showing_instructions = true;
                }
            }

                   
        }

        //Debug.Log("Instructions mode: " + is_showing_instructions + " : " + active_minigame.get_button_state_from_interface());

        if (is_showing_instructions)// && should_scene_advance)
        // start next game
        {
            if (active_minigame.get_button_state_from_interface())
            {
                Debug.Log("Instructions over: Game start!");

                //active_minigame.onButton -= onPress_set_should_scene_advance;


                instructions_text.SetActive(false);

                should_scene_advance = false;
                is_showing_instructions = false;

                active_minigame = minigames[active_minigame_index];

                active_minigame.go();

            }
        }

    }

    public void onPress_set_should_scene_advance()
    {
        should_scene_advance = true;
    }


}
