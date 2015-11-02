#pragma strict
/*
//TODO: convert to C#?
class CSV
{

	static function write_data( filename, array )
	{
		var out_file : StreamWriter;
		out_file = new File.AppendText( filename );
		//var timestamp = getTimestamp();
		var line;
		var didOne = false;
		
		//Debug.Log( filename );
		//Debug.Log( array );
		
		var s : String;
		for( var a in array )
		{
			//Debug.Log( a + " : " + a.GetType() );
			//if( a.GetType() != String && a.GetType() != Number && a.GetType() != float )
			if( a.GetType() != String )
				s = a.ToString();
			else
				s = a;
			//Debug.Log( s );
		
			if( !didOne )		didOne = true;
			else				line += ", ";
			line += s;
		}
		out_file.WriteLine( line );
		out_file.Close();
	}
}
*/