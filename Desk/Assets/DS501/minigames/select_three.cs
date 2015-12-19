using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class select_three : Minigame {

    List<GameObject> things = new List<GameObject>();
    Plane gamePlane;

    int number_selected = 0;
    int number_total = 3;

    float z_distance = 1.4f;

    // need an init
    public override void init_text()
    {
        name = "test_select_red";
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
        foreach( GameObject thing in things )
        {
            Vector3 random_screenpos = new Vector3( Random.Range(0, max_x),
                                                    Random.Range(0, max_y),
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

        // on drag listener (just for fun / demonstration in this task)
        helper.onDrag += (GameObject selected) =>
        {
            Debug.Log("OnDrag");
            Vector3 world_pos = selected.transform.position;
            Vector3 screen_pos = Camera.main.WorldToScreenPoint( world_pos );
            screen_pos.x += helper.screenspace_velocity.x;
            screen_pos.y += helper.screenspace_velocity.y;
            selected.transform.position = misc.ScreenspacePointToPlane( gamePlane, screen_pos );
        };
    }

// need an update
    public override void update()
    {
        // nothing here; everything happened in the listeners
        // could still add some light animation or something here
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
