using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CSV
{
	public static void log<T>(IList<T>array)
	{
		string filename = "log.txt";
		Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

		if (!File.Exists (filename)) {
			File.Create (filename);
		}

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
}
