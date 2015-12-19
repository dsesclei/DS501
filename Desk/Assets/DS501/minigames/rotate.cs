using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class rotate : Minigame {
	
	GameObject target;
	GameObject reference;
	Plane gamePlane;
	
	float z_distance = 1.0f;

    // need an init
    public override void init_text()
    {
        name = "test_rotate";
        instructions = "Rotate right\nmatch left";
    }
    public override void init()
	{
		
		// show the cursor, if you want one
		//helper.showCursor();
		
		// get a plane to put objects on
		gamePlane = helper.getGamePlane( depth: z_distance );
		
		// move the objects in the x/y plane (we may need to define common boundaries somewhere)
		float max_x = Camera.main.pixelWidth,
		max_y = Camera.main.pixelHeight;

        float gutter_x = max_x / 3;
        max_x -= gutter_x;
        float x_offset = gutter_x / 2;

		target = helper.getAnObject();
		reference = GameObject.Instantiate(target);
        x_offset += max_x / 3;
        reference.transform.position = misc.ScreenspacePointToPlane(gamePlane, new Vector3(x_offset, max_y / 2, z_distance));
        x_offset += max_x / 3;
        target.transform.position = misc.ScreenspacePointToPlane(gamePlane, new Vector3(x_offset, max_y / 2, z_distance));
		
		reference.transform.Rotate(new Vector3(Random.Range(0, 359), Random.Range(0, 359), 0));

		
		helper.onRotate += () =>
		{
            //Debug.Log(helper.rotation.eulerAngles);
			target.transform.Rotate(helper.rotation.eulerAngles, relativeTo: Space.World);
		};
		
		helper.onButtonUp += () =>
		{
		};
	}
	
	// need an update
	public override void update()
    {

        float angle = Quaternion.Angle(target.transform.rotation, reference.transform.rotation);
        //Debug.Log( "Angle diff: "  + angle);
        float epsilon = 30;
        if (angle < epsilon)
        {
            helper.success = true;
            helper.end();
        }
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
