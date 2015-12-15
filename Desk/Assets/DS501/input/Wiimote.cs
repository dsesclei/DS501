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

    public static Quaternion    //rot_zero = new Quaternion(),
                                rot_delta = Quaternion.identity,
                                 rotation = Quaternion.identity;

    private static bool[] haveSeenWiimote = new bool[4] {false,false,false,false};

	private static bool has_been_initialized = false;

    private static int sync_stage = 0;
    private static bool is_wmp_active = false;

	// must call init in main run file
	public static void init()//GameObject root ) 
    {
        // things to reset each time
        WiimoteManager.FindWiimotes(); //TODO: run less often?
        sync_stage = 0;

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
        
        //Debug.Log("Wiimotes: " + WiimoteManager.HasWiimote());
        if (!WiimoteManager.HasWiimote()) { return; };//Debug.Log("No Wiimotes found."); return; }

        //update wiimotes; turn IR on for new wiimotes
        WiimoteApi.Wiimote w;
        for (int i = 0; i < WiimoteManager.Wiimotes.Count; i++)
        {
            w = WiimoteManager.Wiimotes[i];
            if (!haveSeenWiimote[i])
            {
                haveSeenWiimote[i] = true;
                //w.SetupIRCamera(IRDataType.BASIC);
                //w.SendStatusInfoRequest();
                //w.SendPlayerLED(true, true, false, false);
            }
            //Debug.Log("  Wiimote " + i + " ext: " + w.current_ext.ToString());
            
            //Debug.Log("Wiimote " + i + ": " + w.hidapi_handle);
        }

        // find wiimote (vs balance board, etc)
        //TODO
        Wiimote.wiimote = WiimoteManager.Wiimotes[0];

        //wiimote.SendPlayerLED(true, false, true, false);
        //wiimote.RequestIdentifyWiiMotionPlus();
        //wiimote.ActivateWiiMotionPlus();
        //wiimote.SetupIRCamera(IRDataType.BASIC);

        // read wiimote data, wiimote plus data
        int ret;
        do
        {
            ret = wiimote.ReadWiimoteData();

            rot_delta = Quaternion.identity;

            if (ret > 0 && wiimote.current_ext == ExtensionController.MOTIONPLUS)
            {
                Vector3 offset = new Vector3(  -wiimote.MotionPlus.PitchSpeed,
                                                wiimote.MotionPlus.YawSpeed,
                                                wiimote.MotionPlus.RollSpeed) / 95f; // Divide by 95Hz (average updates per second from wiimote)
                

                rot_delta = Quaternion.Euler( offset );
                rotation *= rot_delta;

                //TODO: calibrate 0?
                //Debug.Log("read WMP: " + offset + " : " + rot_delta + " : " + rotation);

                //TODO: this could probably use some smoothing
            }
        } while (ret > 0);
        


        // get IR pointer position
        float[] pointer = wiimote.Ir.GetPointingPosition();
        position = new Vector2(pointer[0], pointer[1]);
        //TODO: this could probably use some smoothing, as well

        // get button states
        button_a = wiimote.Button.a;
        button_1 = wiimote.Button.one;

        is_wmp_active = is_extension_motion_plus(wiimote);

		// check for events
        if (position != last_position)          Wiimote.onMove();
        if (rot_delta != Quaternion.identity)   Wiimote.onRotate();
        if (button_a != last_button_a)          Wiimote.onButton_A();
        if (button_1 != last_button_1)          Wiimote.onButton_1();

		// update stored state
        velocity = position - last_position;
        last_position = position;
        last_button_a = button_a;
        last_button_1 = button_1;

        //Debug.Log("WIIMOTE: " + button_1);
        //Debug.Log( "is_wmp_active: " + is_wmp_active );
	}

    public static void activate_IR_and_IMU()
    {
        Debug.Log("Wiimote sync:");
        if (sync_stage == 0)
        {
            Debug.Log("  IR setup");
            wiimote.SetupIRCamera(IRDataType.BASIC);
            //wiimote.RequestIdentifyWiiMotionPlus();
            sync_stage++;
        }

        //if(sync_stage == 1)
        //{
        //    Debug.Log("Wiimote sync stage 2: activate motion plus");
        //    wiimote.ActivateWiiMotionPlus();
        //    sync_stage++;
        //}

        Debug.Log( "WMP status: " + wiimote.wmp_attached + " : " + wiimote.current_ext );
        if (!wiimote.wmp_attached)
        {
            wiimote.RequestIdentifyWiiMotionPlus();
        }
        else if (  ! is_extension_motion_plus( wiimote )    )
        {
            wiimote.ActivateWiiMotionPlus();
        }

    }

    public static bool is_extension_motion_plus( WiimoteApi.Wiimote wiimote )
    {
        return (    wiimote.current_ext == ExtensionController.MOTIONPLUS ||
                    wiimote.current_ext == ExtensionController.MOTIONPLUS_CLASSIC ||
                    wiimote.current_ext == ExtensionController.MOTIONPLUS_NUNCHUCK      );
    }

    public static Action onMove = () => { };//Debug.Log("Wiimote pos: " + position); };
    public static Action onRotate = () => { };//Debug.Log("Wiimote rot: " + position); };
    public static Action onButton_A = () => { Debug.Log("Wiimote button A: " + position); };
    public static Action onButton_1 = () => { activate_IR_and_IMU(); };

}
