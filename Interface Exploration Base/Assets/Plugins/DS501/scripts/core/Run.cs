using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Run : MonoBehaviour
{
	private Dictionary<String,System.Object> config 
										= JSON.read( "config.json" );
    protected int participant_id = 1001;

	protected string data_dir 			= "./data/";
    protected string data_file_prefix 	= "PREFIX_NOT_SET_";
    protected string experiment_name    = "EXPERIMENT_NAME_NOT_SET";
    protected string input_name         = "INPUT_NAME_NOT_SET";
    protected string interface_name     = "INTERFACE_NAME_NOT_SET";
    protected string task_name          = "TASK_NAME_NOT_SET";
    //	private string data_file_name 		= "NAME_NOT_SET";
    //	private string data_file 			=  null;

    private Record_Head_Movement 	measure_head_movement  = null;
	private Record_Mouse 			measure_mouse_movement = null;

	// Use this for initialization
	public virtual void Start ()
	{

		// read default config params
		participant_id = Convert.ToInt32(config["participant_id"]);
		Debug.Log( "Participant ID: " + participant_id );

		// set participant id as random seed (for reproducability)
		UnityEngine.Random.seed = participant_id;

		string timestamp = misc.get_timestamp_string();
		data_file_prefix = data_dir + timestamp + "_" + participant_id 
                         + "_" + experiment_name + "_";

		//default recording:
		measure_head_movement 	= new Record_Head_Movement( 
				data_file_prefix + "head_movement" 
			);
		measure_mouse_movement 	= new Record_Mouse( 
				data_file_prefix + "mouse_movement"
			);

		measure_head_movement.start();  //TODO: only write if changed?  off in debug?
		measure_mouse_movement.start();

	}
}
