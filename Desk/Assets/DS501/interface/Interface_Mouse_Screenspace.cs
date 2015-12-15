using UnityEngine;
using System.Collections;


public class Interface_Mouse_Screenspace : Interface
{
    bool is_active = false;

    Quaternion rot_delta;
    Vector2 position;
    Vector2 pos_delta;
    bool left_click;

    public Vector2 get_ScreenspacePosition()    { return position;   }
    public Vector2 get_ScreenspacePosDelta()    { return pos_delta;  }
    public Quaternion get_Rotation()            { return rot_delta;  }
    public bool get_Button()                    { return left_click; }

    public void init()
    {
        Mouse.init();
    }

    public void on()
    {
        is_active = true;
        Mouse.onMove += update_cursor;

        HeadPose.onMove += update_cursor;
        HeadPose.onRotate += update_cursor;
    }

    public void off()
    {
        is_active = false;
        Mouse.onMove -= update_cursor;

        HeadPose.onMove += update_cursor;
        HeadPose.onRotate += update_cursor;
    }


    public void update()
    {
        rot_delta = Quaternion.Euler(Mouse.velocity.y, Mouse.velocity.x, 0);

        position = Mouse.position;
        pos_delta = Mouse.velocity;
        
        left_click = Mouse.left_down;
    }


    public void update_cursor()
    {
        if (!is_active) return;
        ScreenspaceCursor.update_position(Mouse.position);
    }

}
