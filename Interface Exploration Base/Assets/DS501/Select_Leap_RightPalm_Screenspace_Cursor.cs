using UnityEngine;
using System.Collections;
using System;
using System.Timers;

public class Select_Leap_RightPalm_Screenspace_Cursor
{

    GameObject cursor = GameObject.Find("Software_Cursor");

    Action<GameObject> onSelect = null;

    public void register(Action<GameObject> onSelect) { this.onSelect += onSelect; }
    public void unregister(Action<GameObject> onSelect) { this.onSelect -= onSelect; }

    public Select_Leap_RightPalm_Screenspace_Cursor()
    {
        // init the input method; register with relevant listeners
        LeapMotion.init();
        LeapMotion.onCreate_Right += this.onNewHand;
        LeapMotion.onMove_RightPalm += this.onMove;
        LeapMotion.onPress_RightPalm += this.onPress;
    }

    public void onNewHand()
    {
        cursor.active = true;
    }

    public void onMove()
    {
        Vector3 new_pos = Camera.main.WorldToScreenPoint(LeapMotion.pos_right_palm);
        new_pos.z = 0;
        cursor.transform.position = new_pos;
    }

    public void onPress()
    {
        // delay, to avoid repeated presses
        if (cursor.active == false)
            return;

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
