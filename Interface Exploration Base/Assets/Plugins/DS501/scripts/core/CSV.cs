using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CSV
{
	// writes to generic log file, with timestamps
	public static void log<T>(IList<T>array)
	{
		string filename = "log.txt";
		Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

		//BUG: this was causing a sharing exception?
		//if (!File.Exists (filename)) {
		//	File.Create (filename);
		//}

		int i = 0;
		string[] data = new string[array.Count + 1];
		data [0] = unixTimestamp.ToString ();
		foreach (T item in array) {
			data[i + 1] = item.ToString ();
			i++;
		}

		using (StreamWriter sw = File.AppendText(filename)) 
		{
			string line = String.Join (",", data);
			sw.WriteLine(line);
		}
	}

	// writes to any file, no auto timestamp
	public static void write( string filename, params object[] array )
	{
		int i = 0;
		string[] data = new string[array.Length];
		foreach (object item in array) {
			data[i] = item.ToString ();
			i++;
		}

		using ( StreamWriter sw = File.AppendText(filename) ) 
		{
			string line = String.Join (",", data);
			sw.WriteLine(line);
		}
	}

/*
//TODO: build wrapper classes to open / close files more efficiently?
	// writes to any file, no auto timestamp
	public static void write<T>( File file, IList<T>array )
	{

	}
*/
}
