using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Gel
{
	[ComVisible(true)]
	public sealed class ConsoleApi
	{
		internal ConsoleApi() { }

		public int log(string output)
		{
			Debug.Print(output);
			return String.IsNullOrEmpty(output) ? 0 : output.Length;
		}
		public int warn(string output)
		{
			Debug.Print(output);
			return String.IsNullOrEmpty(output) ? 0 : output.Length;
		}
		public int dir(string output)
		{
			Debug.Print(output);
			return String.IsNullOrEmpty(output) ? 0 : output.Length;
		}
		public int time(string label)
		{
			Debug.Print(label);
			return String.IsNullOrEmpty(label) ? 0 : label.Length;
		}
		public int timeEnd(string label)
		{
			Debug.Print(label);
			return String.IsNullOrEmpty(label) ? 0 : label.Length;
		}
		public int trace(string output)
		{
			Debug.Print(output);
			return String.IsNullOrEmpty(output) ? 0 : output.Length;
		}
		public int assert(string output)
		{
			Debug.Print(output);
			return String.IsNullOrEmpty(output) ? 0 : output.Length;
		}
	}
}
