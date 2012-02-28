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
	}
}
