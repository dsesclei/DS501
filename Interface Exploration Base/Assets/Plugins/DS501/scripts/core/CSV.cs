using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CSV
{
	public static void write_data<T>( String filename, IList<T>array )
	{
		if (!File.Exists (filename)) {
			File.Create (filename);
		}

		int i = 0;
		string[] data = new string[array.Count];
		foreach (T item in array) {
			data[i] = item.ToString ();
			i++;
		}

		using (StreamWriter sw = File.AppendText(filename)) 
		{
			string line = String.Join (",", data);
			sw.WriteLine(line);
		}
	}
}
