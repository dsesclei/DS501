using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Minigame
{

// Things for children to use
    string name = "NO_NAME_SET";

    public Quaternion  rotation;
    public Vector2     screenspace_position;
    public Vector2     screenspace_velocity;
    public bool        action       = false;    // did action start this frame? (Is mouse click now?)
    public bool        action_held  = false;    // is the action active? (Is the button held down?)

    public bool        success = false;

    // listeners
    public Action onAction = () => { };
    public Action onMove   = () => { };
    public Action onRotate = () => { };

    public void setName( string name )  { this.name = name;            }

    public       bool getAction()       { return action;               }
    public       bool getClick()        { return action;               }
    public    Vector2 getPosition( )    { return screenspace_position; }
    public    Vector2 getVelocity()     { return screenspace_velocity; }
    public Quaternion getRotation()     { return rotation;             }
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

    public void show_cursor()    { ScreenspaceCursor.show(); }



// Things for big papa

    private bool action_processed = false;
    private Quaternion last_rotation;
    private Vector2 last_screenspace_position;

    private GameObject[] pool_of_things = misc.get_children_of(GameObject.Find("ThingsPool"));
    private List<GameObject> created = new List<GameObject>();

    public void update()
    {
        // update input method
        //TODO: abstract to interface; add interfaces for other input methods
        Quaternion rot_delta = Quaternion.Euler(Mouse.velocity.y, Mouse.velocity.x, 0);
        //rotation = rotation * rot_delta;
        rotation = rot_delta;

        screenspace_position = Mouse.position;
        screenspace_velocity = Mouse.velocity;

        action_held = Mouse.left_down;

        // aciton is true only if action was clicked this frame
        if (action_processed == false && action_held == true)
        {
            action_processed = true;
            action = true;

            onAction();
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

        // cue next scene

    }

	public GameObject getObject()
    {
        // get random object
        int i = Mathf.RoundToInt(UnityEngine.Random.Range(0, pool_of_things.Length));
        GameObject obj = GameObject.Instantiate( pool_of_things[i] );
        created.Add(obj);
        return obj;
        //TODO: change to just pull and activate / deactiveate if instantiation is slow
    }

    /*
    private void use_Wiimote()
    {

    }
    private void use_Leap()
    {

    }
    private void use_Mouse()
    {
        Mouse.init();
        //TODO: set interface

    }
    private void use_Xbox()
    {

    }
    */
}
