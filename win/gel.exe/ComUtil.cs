using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Gel
{
	static class ComUtil
	{
		public static void DisposeComObject(object obj, bool disposing)
		{
			if (obj == null) return;
			Debug.Assert(Marshal.IsComObject(obj), "Expected COM object.");
			try
			{
				if (!disposing)
					Marshal.FinalReleaseComObject(obj);
				else
					Marshal.ReleaseComObject(obj);
			}
			catch (Exception ex)
			{
				Trace.TraceError("DisposeComObject Error: " + ex.Message);
			}
		}
	}
}
