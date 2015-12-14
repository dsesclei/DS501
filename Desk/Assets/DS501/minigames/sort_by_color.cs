using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sort_by_color : Minigame
{
    List<GameObject> objects = new List<GameObject>();
    Plane plane;

    int total_objects = 6;
    float z_distance = 0.4f;

    public override void init()
    {
        name = "sort_by_color";
        instructions = "Sort the objects by color.";

        helper.showCursor();

        plane = helper.getGamePlane(depth: z_distance);

        for (int i = 0; i < total_objects; i++)
        {
            GameObject obj = helper.getAnObject();
            if (i % 2 == 0)
            {
                setColor(obj, Color.blue);
                obj.name = "blue";
            }
            else
            {
                setColor(obj, Color.red);
                obj.name = "red";
            }
            objects.Add(obj);
        }

        float max_x = Camera.main.pixelWidth,
              max_y = Camera.main.pixelHeight;
        foreach (GameObject obj in objects)
        {
            setRandomPositionOnScreen(obj);
        }
        
        helper.onDrag += (GameObject selected) =>
        {
            Debug.Log("OnDrag");
            Vector3 world_pos = selected.transform.position;
            Vector3 screen_pos = Camera.main.WorldToScreenPoint(world_pos);
            screen_pos.x += helper.screenspace_velocity.x;
            screen_pos.y += helper.screenspace_velocity.y;
            selected.transform.position = misc.ScreenspacePointToPlane(plane, screen_pos);
        };
    }
    
    public override void update()
    {

    }

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

    private void setRandomPositionOnScreen(GameObject obj)
    {
        float max_x = Camera.main.pixelWidth,
              max_y = Camera.main.pixelHeight;
        Vector3 pos;
        Ray ray;
        float depth;

        do
        {
            pos = new Vector3(Random.Range(0, max_x), Random.Range(0, max_y), plane.distance);
            ray = Camera.main.ScreenPointToRay(pos);
            plane.Raycast(ray, out depth);
        } while (Physics.Raycast(ray, depth));

        obj.transform.position = ray.GetPoint(depth);
    }
}
