using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using System;

public class Run_Desk : MonoBehaviour {
    
    protected List<MinigameHelper> minigames;
    protected MinigameHelper active_minigame;
    protected int active_minigame_index;

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

    // logging
    public logger_GameResults logger_results;

    public string id_string = "";
    private bool is_starting_enter_id = false;
    private bool is_entering_id = false;

    public virtual void Start()
    {
        logger_results = new logger_GameResults();

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

        reset();
    }

    public void reset()
    {
        //TODO: get participant id somewhere
        //set_ParticipantID(1001);

        minigames = new List<MinigameHelper>();
        logger_results.participant_id = 1001;

        active_minigame = null;
        active_minigame_index = -1;

        id_string = "";
        is_starting_enter_id = false;
        is_entering_id = false;

        is_showing_instructions = false;
        is_showing_results = false;

        is_ended = false;

        should_scene_advance = false;

        success_text.SetActive(false);
        failure_text.SetActive(false);
        instructions_text.SetActive(false);

        InputTracking.Recenter();

        //start by entering ID
        is_starting_enter_id = true;
    }

    public void set_ParticipantID( int ID )
    {
        Debug.Log( "Register ID: " + ID );

        logger_results.participant_id = ID;
        UnityEngine.Random.seed = ID;
        
        minigames = new List<MinigameHelper>();
        MakeMinigames();
    }

    public virtual void MakeMinigames()
    {
        Interface iface = interface_mouse_screenspace;

        int repeat = 3;

        for (int i = 0; i < repeat; i++)
        {
            AddMinigame(new tutorial_select_three(), iface);
            AddMinigame(new rotate(), iface);
            AddMinigame(new avoid_game(), iface);
            AddMinigame(new sort_by_color(), iface);
        }

        minigames = misc.shuffle<MinigameHelper>(minigames); 
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

        if (Input.GetKeyDown(KeyCode.F5))
        {
            Debug.Log("F5: reset");
            reset();
        }

        if (is_ended) return;

        //TODO: reset button?
        if (is_entering_id || is_starting_enter_id)
        {
            if (is_starting_enter_id)
            {
                id_string = "";
                instructions_text.SetActive(true);
                is_starting_enter_id = false;
                is_entering_id = true;
            }
            if (is_entering_id)
            {
                string new_input = Input.inputString;
                foreach (char c in new_input)
                {
                    if (c == '\b' && id_string.Length > 0)
                    {
                        id_string = id_string.Remove(id_string.Length - 1, 1);
                    }
                    else if (c == '\n' || Input.GetKeyDown(KeyCode.Return))
                    {
                        set_ParticipantID(Int32.Parse(id_string));
                        is_entering_id = false;
                        instructions_text.SetActive(false);
                    }
                    else if (Char.IsNumber(c))
                        id_string += c;
                }
                instructions_text.GetComponent<TextMesh>().text = "Enter ID\n" + id_string;

            }

            return;
        }

        if (active_minigame == null || active_minigame.has_ended)
        // time to show results, process last game, and advance
        {
            //process last game, if exists
            if (active_minigame != null)
            {
                //show results
                if (!is_showing_results)
                {
                    // also do logging
                    logger_results.log( active_minigame_index,
                                        active_minigame.name,
                                        active_minigame.success.ToString(),
                                        active_minigame.elapsed.ToString()  );

                    // then actualy show results
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
                    Application.Quit();
                }
                else// if (!is_showing_instructions)
                // show instructions for next game
                {
                    // get next game
                    active_minigame_index++;
                    Debug.Log("Starting next minigame ... " + active_minigame_index + " of " + minigames.Count);

                    active_minigame = minigames[active_minigame_index];
                    logger_results.input_name = active_minigame.get_InputName();

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
