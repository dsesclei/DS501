using UnityEngine;
using System.Collections;
using System;

public class Xbox : MonoBehaviour
{
	const float speed = 2.5f;
	public static Vector2 position = new Vector2();
	
	private static bool has_been_initialized = false;
	
	public static void init ( )
	{
		if( has_been_initialized )
			return;

		position = new Vector2 (Screen.width / 2, Screen.height / 2);
		
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
		if (Input.GetButton ("Xbox A")) Xbox.onDown_A ();
	}

	public static Action onDown_A = () => { };
}
