using System;

public class Test
{
	static void Main(string[] args)
	{
		SWIGTYPE_p_xwii_monitor mon = xwiimote.xwii_monitor_new(false, false);
		string ent;
		int num = 0;

		for (ent = xwiimote.xwii_monitor_poll(mon); ent != null; ent = xwiimote.xwii_monitor_poll(mon)) {
			Console.WriteLine("Found device #{0}: '{1}'", num, ent);
			num++;
		}
	}
}
