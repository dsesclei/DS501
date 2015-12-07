using UnityEngine;
using System.Collections;
using System;

public class Select_Wiimote_Raycast
{
    Software_Cursor cursor = new Software_Cursor();

    Action<GameObject> onSelect = null;

	public void   register( Action<GameObject> onSelect )	{	this.onSelect += onSelect;	}
	public void unregister( Action<GameObject> onSelect )	{	this.onSelect -= onSelect;	}

	public Select_Wiimote_Raycast()
	{
        // init the input method; register with relevant listeners
        Wiimote.init ();
		//Mouse.onDown_Left   += this.onClick;
        Wiimote.onMove      += this.update_cursor;

        HeadPose.init();
        HeadPose.onMove     += this.update_cursor;
        HeadPose.onRotate   += this.update_cursor;
	}
    ~Select_Wiimote_Raycast()
    {
        Wiimote.destroy();
    }

    public void update_cursor()
    {
        Vector3 screen_pos = new Vector3(  
                                    Camera.main.pixelWidth  * Wiimote.position.x,
                                    Camera.main.pixelHeight * Wiimote.position.y,
                                    0
                                 );

        cursor.update_position( screen_pos );
    }

	public void onClick()
	{
        Ray ray = Camera.main.ScreenPointToRay( Wiimote.position );
		GameObject selected = misc.Raycast_Select_Object ( ray );

		// This is an option
		//if (selected == null)	return;

		onSelect ( selected );
	}
}
