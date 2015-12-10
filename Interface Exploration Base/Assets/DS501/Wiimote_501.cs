using UnityEngine;
using System.Collections;
using System;

using WiimoteApi;

public class Wiimote {

// static is a wrangler
    private static Wiimote instance;


// instance is an individual wiimote

    //private Wiimote_501 w;
    private static WiimoteApi.Wiimote wiimote;

    public static Vector3 position      = new Vector3(),
                          last_position = new Vector3(),
                          velocity      = new Vector3();
    public static bool    button_a        = false,
                          last_button_a   = false,
                          button_1        = false,
                          last_button_1   = false;

    private static bool[] haveSeenWiimote = new bool[4] {false,false,false,false};

	private static bool has_been_initialized = false;

	// must call init in main run file
	public static void init()//GameObject root ) 
	{
        //TODO: this check is maybe not robust to parallel calls?
		if( has_been_initialized )
			return;

        //instance = new Wiimote();

		has_been_initialized = true;
		OnUpdate.register ( delegate() { Wiimote.update(); } );
	}
    public static void destroy()
    {
        foreach ( WiimoteApi.Wiimote w in WiimoteManager.Wiimotes )
        {
            WiimoteManager.Cleanup(w);
        }
    }


	public static void update() 
	{
        WiimoteManager.FindWiimotes(); //TODO: run less often?
        
        Debug.Log("Wiimotes: " + WiimoteManager.HasWiimote());
        if (!WiimoteManager.HasWiimote()) { return; }

        //update wiimotes; turn IR on for new wiimotes
        WiimoteApi.Wiimote w;
        for (int i = 0; i < WiimoteManager.Wiimotes.Count; i++)
        {
            w = WiimoteManager.Wiimotes[i];
            if (!haveSeenWiimote[i])
            {
                haveSeenWiimote[i] = true;
                //w.SetupIRCamera(IRDataType.BASIC);
            }
            Debug.Log("  Wiimote " + i + " ext: " + w.current_ext.ToString());
        }

        // find wiimote (vs balance board, etc)
        //TODO
        Wiimote.wiimote = WiimoteManager.Wiimotes[0];

        // read wiimote data, wiimote plus data
        int ret;
        do
        {
            ret = wiimote.ReadWiimoteData();

            if (ret > 0 && wiimote.current_ext == ExtensionController.MOTIONPLUS)
            {
                Vector3 offset = new Vector3(-wiimote.MotionPlus.PitchSpeed,
                                                wiimote.MotionPlus.YawSpeed,
                                                wiimote.MotionPlus.RollSpeed) / 95f; // Divide by 95Hz (average updates per second from wiimote)
                //wmpOffset += offset;

                //model.rot.Rotate(offset, Space.Self);
            }
        } while (ret > 0);
        

        // get IR pointer position
        float[] pointer = wiimote.Ir.GetPointingPosition();
        position = new Vector2(pointer[0], pointer[1]);

        // get button states
        button_a = wiimote.Button.a;
        button_1 = wiimote.Button.one;

		// check for events
        if (position != last_position) Wiimote.onMove();
        if (button_a != last_button_a) Wiimote.onButton_A();
        if (button_1 != last_button_1) Wiimote.onButton_1();

		// update stored state
        velocity = position - last_position;
        last_position = position;
        last_button_a = button_a;
        last_button_1 = button_1;
	}

    public static Action onMove     = () => { Debug.Log("Wiimote pos: " + position); };
    public static Action onButton_A = () => { Debug.Log("Wiimote button A: " + position); };
    public static Action onButton_1 = () => 
                         { 
                            Debug.Log("Wiimote button 1: init IR tracking");
                            wiimote.SetupIRCamera(IRDataType.BASIC);
                         };

}
