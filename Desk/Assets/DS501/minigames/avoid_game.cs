using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class avoid_game : Minigame {
	
	List<GameObject> walls = new List<GameObject>();
	Bounds bounds;

	float z_distance = 0.4f;
	float gapWidth = 25;
	float speed = 10.0f;

    // need an init
    public override void init_text()
    {
		name = "test_avoid";
		instructions = "Avoid\nthe\nwalls!";
    }
    public override void init()
	{
		
		// show the cursor, if you want one
		helper.showCursor();

		bounds = new Bounds();
		bounds.center = new Vector3(0, 0, 0);
		bounds.size = new Vector3(1, 1, 1);

		helper.success = true;
		
		addWall();

	}
	
	public override void update()
	{
		if (!helper.success) {
			return;
		}
		float angle = Mathf.Atan2(helper.screenspace_position.y, helper.screenspace_position.x - Camera.main.pixelWidth / 2);
		float percent = angle / Mathf.PI;
		foreach (GameObject wall in walls) {
			wall.transform.Translate((percent - 0.5f) * speed, 0, -0.1f * speed);
			Bounds wallBounds = new Bounds();
			wallBounds.center = wall.transform.position;
			wallBounds.size = wall.transform.localScale;
			helper.success = helper.success && !wallBounds.Intersects(bounds);
            if (wallBounds.Intersects(bounds))
            {
                helper.end();
            }
		}

		if (helper.success && walls[walls.Count - 1].transform.position.z < 60) {
			addWall ();
		}
	}
	
	public void addWall() {
		float gapPos = UnityEngine.Random.Range(-25, 25);
		GameObject left = GameObject.CreatePrimitive(PrimitiveType.Cube);
		left.transform.localScale = new Vector3(250, 20, 1);
		left.transform.position = new Vector3(gapPos - 125 - gapWidth / 2, 0, 100);
		walls.Add(left);
		GameObject right = GameObject.CreatePrimitive(PrimitiveType.Cube);
		right.transform.localScale = new Vector3(250, 20, 1);
		right.transform.position = new Vector3(gapPos + 125 + gapWidth / 2, 0, 100);
		walls.Add(right);
	}

	public override void destroy() {
		foreach (GameObject wall in walls) {
			GameObject.Destroy(wall);
		}
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