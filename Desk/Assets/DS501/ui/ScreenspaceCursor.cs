using UnityEngine;
using System.Collections;

public class ScreenspaceCursor {

    public static void init()
    {
        cursor = GameObject.Find("Screenspace_Cursor");
        camera = Camera.main;
    }

    static GameObject cursor    = GameObject.Find("Screenspace_Cursor");
    static Camera camera        = Camera.main;

    static private float cursor_distance = 10;

    static public void update_position( Vector3 screenspace_position )
    {
        
        // desired screenspace position
        Vector3 screen_pos = screenspace_position;

        // project sceenspace position onto canvas plane
        Ray screen_pos_ray = camera.ScreenPointToRay(screen_pos);

        float distance = cursor_distance;
        float scale = 1;

        // check if something's closer than this distance
        RaycastHit hit;
        if (Physics.Raycast(screen_pos_ray, out hit, distance))
        {
            float distance_hit = hit.distance;
            distance = distance_hit > 0 ? distance_hit : cursor_distance;

            // from: http://evrydayvr.srve.io/Oculus_Rift_DK2_-_Unity_Tutorial_Reticle/Reticle.cs
            //if (distance < 1)
            //{
            //    distance *= 1 + 5 * Mathf.Exp(-distance);
            //}
            //transform.localScale = originalScale * distance;

            scale = distance / cursor_distance;
            //Debug.Log("Cursor scale: " + scale + ", " + distance_hit + ", " + distance + ", " + cursor_distance);
        }

        Vector3 new_pos = screen_pos_ray.GetPoint(distance);


        cursor.transform.localScale = new Vector3( scale, scale, scale );

        cursor.transform.position = new_pos;
        cursor.transform.forward  = camera.transform.forward;

    }

    static public void hide()    {  cursor.active = false;  }
    static public void show()    {  cursor.active = true;   }
}
