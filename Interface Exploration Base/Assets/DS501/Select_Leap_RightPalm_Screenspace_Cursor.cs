using UnityEngine;
using System.Collections;
using System;
using System.Timers;

public class Select_Leap_RightPalm_Screenspace_Cursor
{

    //GameObject cursor = GameObject.Find("Software_Cursor");
    //GameObject canvas = GameObject.Find("UI Layer/Canvas"); 

    Software_Cursor cursor = new Software_Cursor();
    Camera camera = Camera.main;

    Action<GameObject> onSelect = null;

    public void register(Action<GameObject> onSelect) { this.onSelect += onSelect; }
    public void unregister(Action<GameObject> onSelect) { this.onSelect -= onSelect; }

    public Select_Leap_RightPalm_Screenspace_Cursor()
    {
        // init the input method; register with relevant listeners
        LeapMotion.init();
        LeapMotion.onCreate_Right += this.onNewHand;
        LeapMotion.onMove_RightPalm += this.update_cursor;
        LeapMotion.onPress_RightPalm += this.onPress;

        HeadPose.init();
        HeadPose.onMove += this.update_cursor;
        HeadPose.onRotate += this.update_cursor;
    }

    public void update_cursor()
    {
        Vector3 screenspace_position = camera.WorldToScreenPoint(LeapMotion.pos_right_palm);
        screenspace_position.z = 0;
        cursor.update_position( screenspace_position );

        Debug.Log("Hands: " + LeapMotion.num_hands + ", PPos: " + LeapMotion.pos_right_palm + ", PPos S: " + screenspace_position);
    }


    public void onNewHand()
    {
        //cursor.active = true;
    }

    /*
    public void onMove()
    {

        RectTransform rect = canvas.GetComponent<RectTransform>();
        RectTransform cursor_rect = cursor.GetComponent<RectTransform>();
        Vector3 new_pos = Mouse.position;
        new_pos.z = cursor_rect.position.z;
        //new_pos.x = (new_pos.x * rect.sizeDelta.x) - (0.5f * rect.sizeDelta.x);
        //new_pos.y = (new_pos.y * rect.sizeDelta.y) - (0.5f * rect.sizeDelta.y);
        new_pos.x = (new_pos.x) - (0.5f * rect.sizeDelta.x);
        new_pos.y = (new_pos.y) - (0.5f * rect.sizeDelta.y);

        cursor.transform.position = new_pos;
        cursor_rect.position = new_pos;
    }
    */

    public void onPress()
    {
        // delay, to avoid repeated presses
        //if (cursor.active == false)
        //    return;

        // visual effect
        //cursor.active = false;
        //Timer t = new System.Timers.Timer(10);
        //t.Elapsed += new ElapsedEventHandler((object s, ElapsedEventArgs e) => { cursor.active = true; });

        // do raycast
        Ray ray = Camera.main.ScreenPointToRay(Camera.main.WorldToScreenPoint(LeapMotion.pos_right_palm));
        GameObject selected = misc.Raycast_Select_Object(ray);

        // This is an option
        //if (selected == null)	return;

        onSelect(selected);
    }
}
