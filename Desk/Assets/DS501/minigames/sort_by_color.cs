using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
            helper.setRandomPositionOnScreen(obj, plane);
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

    public override void destroy()
    {
        Debug.Log("destroy called");
    }

    private bool action_happened = false;
    
    public override void update()
    {
        if (helper.action)
        {
            action_happened = true;
        }
        // Check if the objects are sorted.
        if (action_happened && !helper.action_held)
        {
            // A drag was happening, but it has stopped.
            action_happened = false;

            string leftSideType = "";
            string rightSideType = "";
            bool failure = false;
            foreach (GameObject obj in objects)
            {
                if (obj.transform.position.x < 0)
                {
                    Debug.Log("on left", obj);
                    if (leftSideType == "" || leftSideType == obj.name)
                    {
                        leftSideType = obj.name;
                    }
                    else
                    {
                        failure = true;
                        break;
                    }
                }
                else if (obj.transform.position.x > 0)
                {
                    Debug.Log("on right", obj);
                    if (rightSideType == "" || rightSideType == obj.name)
                    {
                        rightSideType = obj.name;
                    }
                    else
                    {
                        failure = true;
                        break;
                    }
                }
                else
                {
                    failure = true;
                    break;
                }
            }
            helper.success = !failure;
        }
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
}
