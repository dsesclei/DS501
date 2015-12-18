using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class rotate : Minigame {
	
	GameObject target;
	GameObject reference;
	Plane gamePlane;
	
	float z_distance = 0.4f;
	
	// need an init
	public override void init()
	{
		name = "test_rotate";
		instructions = "Rotate the object on the right to match the one on the left";
		
		// show the cursor, if you want one
		helper.showCursor();
		
		// get a plane to put objects on
		gamePlane = helper.getGamePlane( depth: z_distance );
		
		// move the objects in the x/y plane (we may need to define common boundaries somewhere)
		float max_x = Camera.main.pixelWidth,
		max_y = Camera.main.pixelHeight;
		
		target = helper.getAnObject();
		reference = GameObject.Instantiate(target);
		target.transform.position = misc.ScreenspacePointToPlane(gamePlane, new Vector3(max_x * 2 / 3, max_y / 2, z_distance));
		reference.transform.position = misc.ScreenspacePointToPlane(gamePlane, new Vector3(max_x / 3, max_y / 2, z_distance));
		
		reference.transform.Rotate(new Vector3(Random.Range(0, 359), Random.Range(0, 359), Random.Range(0, 359)));
		
		helper.onRotate += () =>
		{
			target.transform.Rotate(helper.rotation.eulerAngles, relativeTo: Space.World);
		};
		
		helper.onButtonUp += () =>
		{
			float angle = Quaternion.Angle(target.transform.rotation, reference.transform.rotation);
			float epsilon = 30;
			if (angle < epsilon) {
				helper.success = true;
				helper.end();
			}
		};
	}
	
	// need an update
	public override void update()
	{
		// nothing here; everything happened in the listeners
		// could still add some light animation or something here
	}

	public override void destroy ()
	{
		GameObject.Destroy(reference);
	}
	
	// a utility function
	private void setColor(GameObject obj, Color color)
	{
		foreach (Renderer r in obj.GetComponents<Renderer>())
		{
			foreach (Material m in r.materials)
			{
				m.color = color;
			}
		}
	}
}
