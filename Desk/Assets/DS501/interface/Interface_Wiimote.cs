using UnityEngine;
using System.Collections;

public class Interface_Wiimote : Interface
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
        Wiimote.init();
    }

    public void on()
    {
        is_active = true;
        Wiimote.onMove += update_cursor;

        HeadPose.onMove += update_cursor;
        HeadPose.onRotate += update_cursor;

    }

    public void off()
    {
        is_active = false;
        Wiimote.onMove -= update_cursor;

        HeadPose.onMove += update_cursor;
        HeadPose.onRotate += update_cursor;
    }


    public void update()
    {
        //Vector3 screenspace_position = Camera.main.WorldToScreenPoint(Wiimote.position);
        //Vector3 screenspace_position = Wiimote.position;
        //screenspace_position.z = 0;
        Vector3 screenspace_position = new Vector3(
                                    Camera.main.pixelWidth * Wiimote.position.x,
                                    Camera.main.pixelHeight * Wiimote.position.y,
                                    0
                                 );


        //rot_delta = Wiimote.rot_delta;
        Vector3 pos_delta = new Vector3(
                                    Camera.main.pixelWidth * Wiimote.velocity.x,
                                    Camera.main.pixelHeight * Wiimote.velocity.y,
                                    0
                                 );
        rot_delta = Quaternion.Euler(pos_delta.y, pos_delta.x, 0);
        //rot_delta = Quaternion.Euler(0,0, Wiimote.rot_delta.eulerAngles.z * 5);

        position = screenspace_position;
        pos_delta = Wiimote.velocity; //TODO: coord space?

        left_click = Wiimote.button_a;
    }


    public void update_cursor()
    {
        if (!is_active) return;
        //Vector3 screenspace_position = Camera.main.WorldToScreenPoint(Wiimote.position);
        //screenspace_position.z = 0;

        Vector3 screenspace_position = new Vector3(
                                    Camera.main.pixelWidth * Wiimote.position.x,
                                    Camera.main.pixelHeight * Wiimote.position.y,
                                    0
                                 );
        //Debug.Log("Wiimote pos:" + screenspace_position);
        ScreenspaceCursor.update_position(screenspace_position);
    }

}