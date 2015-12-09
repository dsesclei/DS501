using UnityEngine;
using System.Collections;

public class ScreenspaceCursor {

    //public static void init()
    //{
    //    cursor = GameObject.Find("Software_Cursor");
    //    camera = Camera.main;
    //}

    static GameObject cursor    = GameObject.Find("Software_Cursor");
    static Camera camera        = Camera.main;

    static private float cursor_distance = 10;

    static public void update_position( Vector3 screenspace_position )
    {
        
        // desired screenspace position
        Vector3 screen_pos = screenspace_position;

        // project sceenspace position onto canvas plane
        Ray screen_pos_ray = camera.ScreenPointToRay(screen_pos);
        float distance = cursor_distance;

        Vector3 new_pos = screen_pos_ray.origin + screen_pos_ray.direction * distance;

        cursor.transform.position = new_pos;
        cursor.transform.forward  = camera.transform.forward;

    }

    static public void hide()    {  cursor.active = false;  }
    static public void show()    {  cursor.active = true;   }
}
