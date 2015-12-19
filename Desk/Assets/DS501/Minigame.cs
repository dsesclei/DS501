using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

public abstract class Minigame
{
    public string name         = "NO_NAME_SET";
    public string instructions = "NO_INSTRUCTIONS_SET";
    public decimal duration = 5.0m;

    public MinigameHelper helper;

    public abstract void init_text();
    public abstract void init();
    public abstract void update();
    public abstract void destroy();
}

public class MinigameHelper
{

// Things for Minigames to use

    public Quaternion  rotation;
    public Vector2     screenspace_position;
    public Vector2     screenspace_velocity;
    public bool        action       = false;    // did action start this frame? (Is mouse click now?)
    public bool        action_held  = false;    // is the action active? (Is the button held down?)

    public bool        success = false;
    public string      instructions = "DO\nSOMETHING";
    public string      name = "A MINIGAME";

    public decimal time_left;
    public Timer timer;
    public Text timer_text;

    public double elapsed = -1;
    public double start_time = -1;

    public GameObject selected = null;

    // listeners
    public             Action onButton = () => { };
	public             Action onButtonUp = () => { };
	public             Action onMove   = () => { };
    public             Action onRotate = () => { };
    public Action<GameObject> onSelect = (GameObject obj) => { };
    public Action<GameObject> onDrag   = (GameObject obj) => { };

    
    public void showCursor()    { ScreenspaceCursor.show(); }

    public GameObject getAnObject()
    {
        // get random object
        int i = Mathf.RoundToInt(UnityEngine.Random.Range(0, pool_of_things.Length));
        GameObject obj = GameObject.Instantiate(pool_of_things[i]);
        created.Add(obj);
        return obj;
        //TODO: change to just pull and activate / deactiveate if instantiation is slow?
    }

    public Plane getGamePlane( float depth = 2.0f)
    {
        return new Plane( new Vector3(0, 0, -1), depth );
    }

    public void setRandomPositionOnScreen(GameObject obj, Plane plane)
    {
        float max_x = Camera.main.pixelWidth,
              max_y = Camera.main.pixelHeight;
        Vector3 pos;
        Ray ray;
        float depth;

        do
        {
            pos = new Vector3(UnityEngine.Random.Range(0, max_x), UnityEngine.Random.Range(0, max_y), plane.distance);
            ray = Camera.main.ScreenPointToRay(pos);
            plane.Raycast(ray, out depth);
        } while (Physics.Raycast(ray, depth));

        obj.transform.position = ray.GetPoint(depth);
    }

    public string get_InputName()
    {
        return inface.get_Name();
    }

    //public Vector3 ScreenspacePointToGamePlane(Vector3 screenspace_position)
    //{
    //    return misc.ScreenspacePointToPlane(gamePlane, screenspace_position);
    //}


    // Things for the helper's internals

    public MinigameHelper( Minigame minigame, Interface inface, GameObject[] pool_of_things )
    {
        this.pool_of_things = pool_of_things;

        this.minigame = minigame;
        minigame.helper = this;
        minigame.init_text();
        name = minigame.name;
        instructions = minigame.instructions;

        this.inface = inface;
        inface.init();


        //gamePlane = new Plane( new Vector3( 0,0,1 ), 0.3f );

        // add more event monitoring
        onButton += () => 
        {
            Ray ray = Camera.main.ScreenPointToRay(inface.get_ScreenspacePosition());
            selected = misc.Raycast_Select_Object(ray);
            //Debug.Log("Raycast: " + selected);
            if (selected != null)
            {
                onSelect(selected);
            }
        };

        onMove += () =>
        {
            //Debug.Log("Drag? " + action_held + " : " + selected);
            if (selected != null && action_held)
            {
                onDrag(selected);
            }
        };

    }

	private bool last_action_held = false;
    private bool action_processed = false;
    private Quaternion last_rotation;
    private Vector2 last_screenspace_position;

    private GameObject[] pool_of_things = null;// misc.get_children_of(GameObject.Find("ThingsPool"));
    private List<GameObject> created = new List<GameObject>();

    private Interface inface = null; //TODO: pass one in in constructor?
    private Minigame minigame = null;
    public bool has_ended = false;
    public bool has_started = false;

    private bool have_seen_success = false;

    //private Plane gamePlane;
    public bool get_button_state_from_interface() //TODO: quick hack
    {
        return inface.get_Button();
    }

    public void prestart()
    {
        // init input
        inface.on();
        //update();

        //register update function
        OnUpdate.register(this.update);
    }

    public void go()
    {
        Debug.Log("  Start minigame: " + minigame.name);

        minigame.init();

        start_time = misc.get_timestamp();

        //start timer
        timer_text = GameObject.Find("TimerText").GetComponent<Text>();
        //Debug.Log(timer_text.text, timer_text);
        time_left = this.minigame.duration;
        timer = new Timer(100); // Update timer every 0.1s
        timer.Elapsed += (object sender, ElapsedEventArgs e) =>
        {
            // Update the timer.
            time_left = time_left - 0.1m;
        };
        timer.Start();

        // zero the stored rotation
        rotation = new Quaternion();

        has_started = true;
    }

    public void update()
    {
        // update input method
        inface.update();
        rotation = inface.get_Rotation();

        screenspace_position = inface.get_ScreenspacePosition();
        screenspace_velocity = inface.get_ScreenspacePosDelta();
        
        action_held = inface.get_Button();

        if (!has_started) return;

        if(!have_seen_success)
        {
            have_seen_success = true;
            elapsed = misc.get_timestamp() - start_time;
        }

        //Debug.Log(time_left);
        timer_text.text = time_left.ToString();
        if (time_left <= 0 && !has_ended)
        {
            timer.Stop();
            end();
            return;
        }
        //Debug.Log("Actions etc: " + action +" : "+ action_held);

        // aciton is true only if action was clicked this frame
        if (action_processed == false && action_held == true)
        {
            action_processed = true;
            action = true;

            onButton();
        }
        else
            action = false;

        if (!action_held && !action)
            action_processed = false;

		if (!action_held && last_action_held) {
			onButtonUp();
		}
		last_action_held = action_held;

        //Debug.Log("Is Rotation? " + last_rotation + " : " + rotation);

        // fire other events
        if (             last_rotation != rotation             )    onRotate();
        if ( last_screenspace_position != screenspace_position )    onMove();

        // remember state
        last_rotation = rotation;
        last_screenspace_position = screenspace_position;


        minigame.update();
    }

    public void end()
    {
        OnUpdate.unregister(this.update);

        // cleanup
        this.minigame.destroy();
        ScreenspaceCursor.hide();
        //current_interface.disable();

        foreach ( GameObject obj in created )
        {
            obj.SetActive(false);
            GameObject.Destroy(obj);
        }

        inface.off();

        // show success / failure animation
        // TODO: 
        if ( success)    Debug.Log("SUCCESS");
        else             Debug.Log("FAILURE");

        //kill circular reference
        this.minigame = null;

        // cue next scene
        has_ended = true;
    }
    
}
