using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public abstract class Minigame
{
    public string name         = "NO_NAME_SET";
    public string instructions = "NO_INSTRUCTIONS_SET";

    public MinigameHelper helper;

    public abstract void update();
    public abstract void init();
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

    public GameObject selected = null;

    // listeners
    public             Action onButton = () => { };
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

    public Plane getGamePlane( float depth = 0.4f)
    {
        return new Plane( new Vector3(0, 0, -1), depth );
    }

    //public Vector3 ScreenspacePointToGamePlane(Vector3 screenspace_position)
    //{
    //    return misc.ScreenspacePointToPlane(gamePlane, screenspace_position);
    //}


// Things for the helper's internals

    public MinigameHelper( Minigame minigame, Interface inface )
    {

        this.minigame = minigame;
        minigame.helper = this;
        minigame.init();

        this.inface = inface;
        inface.init();

        //gamePlane = new Plane( new Vector3( 0,0,1 ), 0.3f );

        // add more event monitoring
        onButton += () => 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            selected = misc.Raycast_Select_Object(ray);
            Debug.Log("Raycast: " + selected);
            if (selected != null)
            {
                onSelect(selected);
            }
        };

        onMove += () =>
        {
            Debug.Log("Drag? " + action_held + " : " + selected);
            if (selected != null && action_held)
            {
                onDrag(selected);
            }
        };

    }


    private bool action_processed = false;
    private Quaternion last_rotation;
    private Vector2 last_screenspace_position;

    private GameObject[] pool_of_things = misc.get_children_of(GameObject.Find("ThingsPool"));
    private List<GameObject> created = new List<GameObject>();

    private Interface inface = null; //TODO: pass one in in constructor?
    private Minigame minigame = null;
    public bool has_ended = false;

    //private Plane gamePlane;


    public void go()
    {
        //register update function
        OnUpdate.register(this.update);

        //start timer

        // zero the stored rotation
        rotation = new Quaternion();

        // init input
        inface.on();
        //update();
    }

    public void update()
    {
        // update input method
        inface.update();
        rotation = inface.get_Rotation();

        screenspace_position = inface.get_ScreenspacePosition();
        screenspace_velocity = inface.get_ScreenspacePosDelta();

        action_held = inface.get_Button();

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
        else            Debug.Log("FAILURE");

        //kill circular reference
        this.minigame = null;

        // cue next scene
        has_ended = true;
    }
    
}
