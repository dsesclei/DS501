using UnityEngine;
using System.Collections;
using System;
using Leap;

public class LeapMotion {

    private static bool has_been_initialized = false;
    private static HandController hand_controller;

    private static HandModel left  = null;
    private static HandModel right = null;

    public static Vector3 pos_right_palm;
    public static Vector3 vel_right_palm;

    public static void init()//GameObject hand_controller ) 
    {
        GameObject root = GameObject.Find("Root");
        hand_controller = root.GetComponentsInChildren<HandController>()[0];
        //Mouse.root = root;

        //TODO: this check is maybe not robust to parallel calls?
        if (has_been_initialized)
            return;

        has_been_initialized = true;
        OnUpdate.register(delegate() { LeapMotion.update(); });
    }


    public static void update()
    {
        HandModel new_left = null;
        HandModel new_right = null;


        HandModel[] hands = hand_controller.GetAllGraphicsHands();

        //Debug.Log( "Hands detected: " + hands.Length );

        //TODO: actually test left / right
        //foreach( HandModel h in hands )
        //{
        //    // is left or right?
        //}

        //TODO: this is a hack
        if (hands.Length > 0)
            new_right = hands[0];

        Vector3 new_pos_right_palm = new Vector3(0, 0, 0);
        if( new_right != null )
            new_pos_right_palm = new_right.GetPalmPosition();

        //if (new_right != null)
        //    Debug.Log( "Finger: " + new_right.fingers[1].GetTipPosition() );

        if( right == null && new_right != null )
            onCreate_Right();

        right = new_right;

        if (right != null && pos_right_palm != new_pos_right_palm)
            onMove_RightPalm();

        vel_right_palm = new_pos_right_palm - pos_right_palm;
        pos_right_palm = new_pos_right_palm;

        if (Mathf.Abs(vel_right_palm.z) > 0.005)
            onPress_RightPalm();
    }

    public static Action onCreate_Right = () => {  };
    public static Action onMove_RightPalm = () => { };
    public static Action onPress_RightPalm = () => { };
}
