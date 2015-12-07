using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using MiniJSON;

public class JSON
{
	public static Dictionary<String,System.Object> read( string filename )
	{
		StreamReader in_file = File.OpenText( filename );
		var data_JSON = in_file.ReadToEnd( );
		in_file.Close();
		
		return MiniJSON.Json.Deserialize( data_JSON ) as Dictionary<String,System.Object>;
	}
}
