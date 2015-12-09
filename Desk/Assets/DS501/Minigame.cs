using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Minigame
{
    string name         = "NO_NAME_SET";
    string instructions = "NO_INSTRUCTIONS_SET";
    MinigameHelper helper;

    public void set_helper(MinigameHelper helper) { this.helper = helper; }
}

public class MinigameHelper
{

// Things for children to use

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

    
    /*
    public void setName( string name )  { this.name = name; }
    public void setInstructions(string instructions) { this.instructions = instructions; }

    public       bool getButton()       { return action;               }
    public       bool getClick()        { return getButton();          }
    public    Vector2 getPosition( )    { return screenspace_position; }
    public    Vector2 getVelocity()     { return screenspace_velocity; }
    public Quaternion getRotation()     { return rotation;             }
    public GameObject getSelected()     { return selected;             }
    public       void theyWon()         { this.success = true;         }

    public void go()
    {
        //register update function
        OnUpdate.register(this.update);

        //start timer

        // zero the stored rotation
        rotation = new Quaternion();

        // init input
        Mouse.init();
        update();
    }
    */

    public void show_cursor()    { ScreenspaceCursor.show(); }



// Things for big papa
    
    public MinigameHelper( Minigame minigame, Interface inface )
    {

        this.minigame = minigame;
        minigame.set_helper(this);
        this.inface = inface;

        // add more event monitoring
        onButton = () => 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            GameObject selected = misc.Raycast_Select_Object(ray);
            if (selected != null)
            {
                onSelect(selected);
            }
        };

        onMove = () =>
        {
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

    public void update()
    {

        // update input method
        //TODO: abstract to interface; add interfaces for other input methods
        rotation = inface.get_Rotation();

        screenspace_position = inface.get_ScreenspacePosition();
        screenspace_velocity = inface.get_ScreenspacePosDelta();

        action_held = inface.get_Button();

        // aciton is true only if action was clicked this frame
        if (action_processed == false && action_held == true)
        {
            action_processed = true;
            action = true;

            onButton();
        }
        else
            action = false;

        // fire other events
        if (             last_rotation != rotation             )    onRotate();
        if ( last_screenspace_position != screenspace_position )    onMove();

        // remember state
        last_rotation = rotation;
        last_screenspace_position = screenspace_position;
    }

    public void end()
    {

        OnUpdate.unregister(this.update);

        // cleanup
        ScreenspaceCursor.hide();
        //current_interface.disable();

        foreach( GameObject obj in created )
        {
            obj.SetActive(false);
            GameObject.Destroy(obj);
        }

        // show success / failure animation
        // TODO: 
        if( success)    Debug.Log("SUCCESS");
        else            Debug.Log("FAILURE");

        //kill circular reference
        this.minigame = null;

        // cue next scene

    }

	public GameObject getAnObject()
    {
        // get random object
        int i = Mathf.RoundToInt(UnityEngine.Random.Range(0, pool_of_things.Length));
        GameObject obj = GameObject.Instantiate( pool_of_things[i] );
        created.Add(obj);
        return obj;
        //TODO: change to just pull and activate / deactiveate if instantiation is slow
    }
    
}
