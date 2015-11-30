using UnityEngine;
using System.Collections;

public class Software_Cursor {

    GameObject cursor = GameObject.Find("Software_Cursor");
    
    // 
    private float cursor_distance = 10;
    //private Vector3 cursor_transform_position = cursor.transform.position;
    Camera camera = Camera.main;

    public void update_position( Vector3 screenspace_position )
    {
        
        // desired screenspace position
        Vector3 screen_pos = screenspace_position;
        //screen_pos.x = Mouse.position.x;
        //screen_pos.y = Mouse.position.y;
        //screen_pos.z = 0;

        /*
        // find the canvas plane
        Vector3 plane_point = camera.transform.position + camera.transform.forward.normalized * cursor_distance;
        Plane plane = new Plane( Camera.main.transform.forward, cursor.transform.position );

        // project sceenspace position onto canvas plane
        Ray screen_pos_ray = camera.ScreenPointToRay(screen_pos);
        float distance;
        plane.Raycast( screen_pos_ray, out distance );       
        //distance = cursor_distance;

        Vector3 new_pos = screen_pos_ray.origin + screen_pos_ray.direction * distance;
        //new_pos.z = cursor_distance;
        */

        // find the canvas plane
        //Vector3 plane_point = camera.transform.position + camera.transform.forward.normalized * cursor_distance;
        //Plane plane = new Plane(Camera.main.transform.forward, cursor.transform.position);

        // project sceenspace position onto canvas plane
        Ray screen_pos_ray = camera.ScreenPointToRay(screen_pos);
        float distance;
        //plane.Raycast(screen_pos_ray, out distance);
        distance = cursor_distance;

        Vector3 new_pos = screen_pos_ray.origin + screen_pos_ray.direction * distance;
        //new_pos.z = cursor_distance;

        cursor.transform.position = new_pos;
        cursor.transform.forward = camera.transform.forward;
    }

	//public void onClick()
	//{
	//	Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
	//	GameObject selected = misc.Raycast_Select_Object ( ray );
    //
	//	// This is an option
	//	//if (selected == null)	return;
    //
	//	onSelect (selected);
	//}
}
