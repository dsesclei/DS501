using UnityEngine;
using System.Collections;
using System;
using Leap;

public class LeapMotion {

    private static bool has_been_initialized = false;
    private static HandController hand_controller;

    public static int num_hands = 0;
    private static HandModel left  = null;
    private static HandModel right = null;

    public static Vector3 pos_right_palm;
    public static Vector3 vel_right_palm;

    public static Quaternion rot_right_palm = new Quaternion(),
                             new_rot_right_palm = new Quaternion(),
                             rot_right_palm_delta = new Quaternion();
    public static bool did_have_rot_right_palm = false;

    public static bool is_right_palm_pressed = false;

    public static Vector3 new_pos_right_palm;

    public static Vector3 new_pos_right_pointer = new Vector3(),
                              pos_right_pointer = new Vector3(),
                              vel_right_pointer = new Vector3();
    public static bool has_fingertip = false;
    public static bool is_pressed_right_pointer = false;

    public static void init()//GameObject hand_controller ) 
    {
        // reset whenever init is called
        GameObject root = GameObject.Find("root");
        hand_controller = root.GetComponentsInChildren<HandController>()[0];

        // but don't register multiple times with onUpdate
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
        num_hands = hands.Length;
        //Debug.Log( "Hands detected: " + hands.Length );

        //TODO: actually test left / right
        //foreach( HandModel h in hands )
        //{
        //    // is left or right?
        //}

        //TODO: this is a hack
        if (hands.Length > 0)
            new_right = hands[0];

        //Vector3 new_pos_right_palm = new Vector3(0, 0, 0);
        if (new_right != null)
        {
            new_pos_right_palm = new_right.GetPalmPosition();//.ToUnityScaled();
            new_rot_right_palm = new_right.GetPalmRotation();
            did_have_rot_right_palm = true;

            FingerModel right_pointer = new_right.fingers[1];//GetLeapHand().Finger(1);
            if( right_pointer.isActiveAndEnabled )
            {
                new_pos_right_pointer = right_pointer.GetTipPosition();//LeapToUnityWorld(right_pointer.GetTipPosition());
                //vel_right_pointer = new_pos_right_pointer - pos_right_pointer;
                has_fingertip = true;

                //Debug.Log(new_pos_right_pointer);
            }
            else
            {
                has_fingertip = false;
                Debug.Log("No Fingertip! " + new_right.GetLeapHand().Fingers.Count);
            }
        }

        //if (new_right != null)
        //    Debug.Log( "Finger: " + new_right.fingers[1].GetTipPosition() );

        if( right == null && new_right != null )
            onCreate_Right();

        right = new_right;

        if( right == null )
        // set no info if no hand
        {
            vel_right_palm.Set(0, 0, 0);
            rot_right_palm_delta = Quaternion.identity;
            did_have_rot_right_palm = false;
            return;
        }

        if (right != null && pos_right_palm != new_pos_right_palm)
            onMove_RightPalm();

        vel_right_palm = new_pos_right_palm - pos_right_palm;
        pos_right_palm = new_pos_right_palm;

        // only calc rot_delta if we had a hand last frame
        if (did_have_rot_right_palm)
            rot_right_palm_delta = Quaternion.Inverse(rot_right_palm) * new_rot_right_palm;

        if (has_fingertip && vel_right_pointer != Vector3.zero)
            onMove_RightPointer();

        if (Mathf.Abs(vel_right_palm.z) > 0.005)
        {
            is_right_palm_pressed = true;
            onPress_RightPalm();
        }
        else
            is_right_palm_pressed = false;


        if (Mathf.Abs(vel_right_pointer.z) > 0.005)
        {
            is_pressed_right_pointer = true;
            onMove_RightPointer();
        }
        else
            is_pressed_right_pointer = false;
    }

    //public static Vector3 LeapToUnity_Position( Leap.Vector v )
    //{
    //   return new Vector3 ( v.x, v.y, -v.z );
    //}
    public static Vector3 LeapToUnityWorld(Leap.Vector v)
    {
        //return new Vector3(v.x, v.y, v.z);
        Vector3 unityPosition = v.ToUnityScaled(false);
        Vector3 worldPosition = hand_controller.transform.TransformPoint(unityPosition);
        return worldPosition;
    }

    public static Action onCreate_Right = () => {  };
    public static Action onMove_RightPalm = () => { };
    public static Action onMove_RightPointer = () => { };
    public static Action onPress_RightPalm = () => { };
    public static Action onPress_RightPointer = () => { };
}
