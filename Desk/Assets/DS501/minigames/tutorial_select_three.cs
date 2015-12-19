using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tutorial_select_three : Minigame {

    List<GameObject> things = new List<GameObject>();
    Plane gamePlane;

    int number_selected = 0;
    int number_total = 3;

    float z_distance = 1.4f;

    // need an init
    public override void init_text()
    {

        name = "Tutorial Select Three";
        instructions = "Select Them All";

    }
    public override void init()
    {

        // show the cursor, if you want one
        helper.showCursor();

        // get a plane to put objects on
        gamePlane = helper.getGamePlane( depth: z_distance );

        // get some objects (you can also just make some or whatever)
        for (int i = 0; i < number_total; i++)
        {
            GameObject thing = helper.getAnObject();
            setColor( thing, Color.yellow );
            thing.name = "not selected";
            things.Add( thing );
        }

        // move the objects in the x/y plane (we may need to define common boundaries somewhere)
        float max_x = Camera.main.pixelWidth,
              max_y = Camera.main.pixelHeight;
        float x_gutter = max_x / 3,
              x_area = max_x - x_gutter;
        float offset = x_gutter/2,
              offset_delta = x_area / 3;
        foreach( GameObject thing in things )
        {
            offset += offset_delta;
            Vector3 random_screenpos = new Vector3( offset,
                                                    max_y/2,
                                                    z_distance
                                                  );
            //TODO: sometimes these are in the desk; should not use bottom of screen?

            thing.transform.position = misc.ScreenspacePointToPlane( gamePlane, random_screenpos );
        }

        // on select listener
        helper.onSelect += (GameObject selected) =>
        {
            // mark as selected; check if we've selected it before, count it if not
            setColor(selected, Color.red);
            if( selected.name == "not selected" )
            {
                number_selected += 1;
                Debug.Log("SELECTED: " + number_selected +" of "+ number_total );
                selected.name = "selected";
            }

            // check win state
            if (number_selected >= number_total)
            {
                helper.success = true;
                helper.end();
            }
        };

    }

// need an update
    public override void update()
    {
        foreach (GameObject thing in things)
        {
            Vector3 random_rotation = new Vector3( UnityEngine.Random.Range(0, 1),
                                                   UnityEngine.Random.Range(0, 1),
                                                   UnityEngine.Random.Range(0, 1)
                                                 );

            thing.transform.rotation *= Quaternion.Euler(random_rotation);
        }
    }

    public override void destroy()
    {

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
