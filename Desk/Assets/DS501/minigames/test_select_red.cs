using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class test_select_red : Minigame {

    List<GameObject> things = new List<GameObject>();
    int number_selected = 0;
    int number_total = 3;
	
// need an init
    public override void init()
    {
        name = "test_select_red";
        instructions = "Select Them All";

        // show the cursor, if you want one
        helper.showCursor();

        // get some objects (you can also just make some or whatever)
        for (int i = 0; i < number_total; i++)
        {
            GameObject thing = helper.getAnObject();
            setColor( thing, Color.yellow );
            thing.name = "not selected";
            things.Add( thing );
        }

        // move the objects in the x/y plane (we may need to define common boundaries somewhere)
        float z_distance = 0.3f;
        float max_x = Camera.main.pixelWidth,
              max_y = Camera.main.pixelHeight;
        foreach( GameObject thing in things )
        {
            Vector3 random_screenpos = new Vector3( Random.Range(0, max_x),
                                                    Random.Range(0, max_y),
                                                    z_distance
                                                  );
            thing.transform.position = Camera.main.ScreenToWorldPoint( random_screenpos );
            //TODO: this could push out to a plane, not just a set z
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
                Debug.Log( "SUCCESS!" );
                helper.success = true;
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
            selected.transform.position = Camera.main.ScreenToWorldPoint(screen_pos);

            //TODO: this could move within a plane, not just a set z
        };
    }

// need an update
    public override void update()
    {
        // nothing here; everything happened in the listeners
        // could still add some light animation or something here
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
