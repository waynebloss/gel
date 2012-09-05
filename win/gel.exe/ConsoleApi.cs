using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Gel
{
	public sealed class ConsoleApi
	{
		internal ConsoleApi() { }

		public void log(string output)
		{
			Debug.Print(output);
		}
		public void warn(string output)
		{
			Debug.Print(output);
		}
		public void dir(string output)
		{
			Debug.Print(output);
		}
		public void time(string label)
		{
			Debug.Print(label);
		}
		public void timeEnd(string label)
		{
			Debug.Print(label);
		}
		public void trace(string output)
		{
			Debug.Print(output);
		}
		public void assert(string output)
		{
			Debug.Print(output);
		}
	}
}
