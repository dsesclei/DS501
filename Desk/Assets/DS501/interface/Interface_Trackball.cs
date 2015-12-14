using UnityEngine;
using System.Collections;

public class Intercace_Trackball : Interface
{
    bool is_active = false;

    Quaternion rot_delta;
    Vector2 position;
    Vector2 pos_delta;
    bool left_click;

    public Vector2 get_ScreenspacePosition() { return position; }
    public Vector2 get_ScreenspacePosDelta() { return pos_delta; }
    public Quaternion get_Rotation() { return rot_delta; }
    public bool get_Button() { return left_click; }

    public void init()
    {
        TrackBall.init();
    }

    public void on()
    {
        is_active = true;
        TrackBall.onMove += update_cursor;
    }

    public void off()
    {
        is_active = false;
        TrackBall.onMove -= update_cursor;
    }


    public void update()
    {
        rot_delta = Quaternion.Euler(TrackBall.velocity.y, TrackBall.velocity.x, 0);

        position = TrackBall.position;
        pos_delta = TrackBall.velocity;

        left_click = TrackBall.last_left_down;
    }


    public void update_cursor()
    {
        ScreenspaceCursor.update_position(TrackBall.position);
    }

}

