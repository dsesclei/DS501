using UnityEngine;
using System.Collections;

public class Interface_Xbox : Interface
{
    bool is_active = false;

    Quaternion rot_delta;
    Vector2 position;
    Vector2 pos_delta;
    bool button;

    public Vector2 get_ScreenspacePosition() { return position; }
    public Vector2 get_ScreenspacePosDelta() { return pos_delta; }
    public Quaternion get_Rotation() { return rot_delta; }
    public bool get_Button() { return button; }

    public void init()
    {
        Mouse.init();
    }

    public void on()
    {
        is_active = true;
        Xbox.onMove += update_cursor;
    }

    public void off()
    {
        is_active = false;
        Xbox.onMove -= update_cursor;
    }


    public void update()
    {
        rot_delta = Quaternion.Euler(Xbox.position_delta.y, Xbox.position_delta.x, 0);

        position = Xbox.position;
        pos_delta = Xbox.position_delta;

        button = Xbox.button_down;
    }


    public void update_cursor()
    {
        if (!is_active) return;

        //Debug.Log("Xbox: update mouse: " + Xbox.position);
        ScreenspaceCursor.update_position(Xbox.position);
    }

}

