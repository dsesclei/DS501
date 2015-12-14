using UnityEngine;
using System.Collections;
using System;

public class Xbox : MonoBehaviour
{
	const float speed = 15f;
	public static Vector2 position = new Vector2();
    public static Vector2 last_position = new Vector2();
    public static Vector2 position_delta = new Vector2();
    public static bool button_down = false;
	
	private static bool has_been_initialized = false;

    private static Vector2 vec2_zero = new Vector2(0, 0);
	
	public static void init ( )
    {
        // always reset position when init is called
        position = new Vector2(Screen.width / 2, Screen.height / 2);
        last_position = position;
        position_delta = last_position - position;

        // but don't register multiple times with OnUpdate
		if( has_been_initialized )
			return;
		
		has_been_initialized = true;
		OnUpdate.register ( delegate() { Xbox.update(); } );
	}
	
	
	public static void update ()
	{

		float horizontal_axis = Input.GetAxis ("Horizontal");
		float vertical_axis = Input.GetAxis ("Vertical") * -1;
		position += speed * new Vector2(horizontal_axis, vertical_axis);
		position.x = Math.Max (0, position.x);
		position.x = Math.Min (position.x, Screen.width);
		position.y = Math.Max (0, position.y);
		position.y = Math.Min (position.y, Screen.height);

        button_down = Input.GetButton("Xbox A");
        if (Input.GetButton("Xbox A"))    Xbox.onDown_A();

        position_delta = last_position - position;
        last_position = position;

        if (position_delta != vec2_zero) Xbox.onMove();

        //Debug.Log("XBOX: " + position_delta);
	}

    public static Action onDown_A = () => { };
    public static Action onMove = () => { };
}
