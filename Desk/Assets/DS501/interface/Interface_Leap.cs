using UnityEngine;
using System.Collections;

public class Interface_Leap : Interface
{
    bool is_active = false;

    Quaternion rot_delta;
    Vector2 position;
    Vector2 pos_delta;
    bool action;

    public string get_Name() { return "Leap";  }
    public Vector2 get_ScreenspacePosition() { return position; }
    public Vector2 get_ScreenspacePosDelta() { return pos_delta; }
    public Quaternion get_Rotation() { return rot_delta; }
    public bool get_Button() { return action; }

    public void init()
    {
        LeapMotion.init();
    }

    public void on()
    {
        is_active = true;
        LeapMotion.onMove_RightPalm += update_cursor;
        //LeapMotion.onMove_RightPointer += update_cursor;

        HeadPose.onMove += update_cursor;
        HeadPose.onRotate += update_cursor;
    }

    public void off()
    {
        is_active = false;
        LeapMotion.onMove_RightPalm -= update_cursor;
        //LeapMotion.onMove_RightPointer -= update_cursor;

        HeadPose.onMove += update_cursor;
        HeadPose.onRotate += update_cursor;
    }


    public void update()
    {
        
        rot_delta = LeapMotion.rot_right_palm_delta;

        Vector3 screenspace_position = Camera.main.WorldToScreenPoint(LeapMotion.pos_right_palm);
        screenspace_position.z = 0;

        position = screenspace_position;
        pos_delta = LeapMotion.vel_right_palm; //TODO: what coord space?

        action = LeapMotion.is_right_palm_pressed;
        

        /*
        rot_delta = LeapMotion.rot_right_palm_delta;

        Vector3 screenspace_position = Camera.main.WorldToScreenPoint(LeapMotion.pos_right_pointer);
        screenspace_position.z = 0;

        position = screenspace_position;
        pos_delta = LeapMotion.vel_right_pointer; //TODO: what coord space?

        action = LeapMotion.is_pressed_right_pointer;
         */
    }


    public void update_cursor()
    {
        if (!is_active) return;

        //Vector3 screenspace_position = Camera.main.WorldToScreenPoint(LeapMotion.pos_right_pointer);
        Vector3 screenspace_position = Camera.main.WorldToScreenPoint(LeapMotion.pos_right_palm);
        screenspace_position.z = 0;
        //Debug.Log("LEAP UPDATE CURSOR: " + screenspace_position);
        ScreenspaceCursor.update_position(screenspace_position);
    }

}
