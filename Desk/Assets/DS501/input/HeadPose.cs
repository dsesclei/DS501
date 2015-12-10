using UnityEngine;
using System.Collections;
using System;
using UnityEngine.VR;

public class HeadPose
{
    //public static GameObject root = null;

    public static Vector3 position      = new Vector3(),
                          last_position = new Vector3(),
                          velocity      = new Vector3();
    public static Quaternion rotation      = new Quaternion(),
                             last_rotation = new Quaternion();
                             //TODO: angular velocity?

    private static bool has_been_initialized = false;

    // must call init in main run file
    public static void init()//GameObject root ) 
    {
        //TODO: this check is maybe not robust to parallel calls?
        if (has_been_initialized)
            return;

        has_been_initialized = true;
        OnUpdate.register( delegate() { HeadPose.update(); } );
    }


    public static void update()
    {
        rotation = InputTracking.GetLocalRotation(0);
        position = InputTracking.GetLocalPosition(0);

        // check for events
        if (position != last_position) HeadPose.onMove();
        if (rotation != last_rotation) HeadPose.onRotate();

        // update stored state
        velocity = position - last_position; //TODO: factor elapsed time?
        last_position = position;

        last_rotation = rotation;

    }

    public static Action onMove     = () => { };
    public static Action onRotate   = () => { };

}
