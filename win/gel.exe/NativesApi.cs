using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Gel.Scripting;

namespace Gel
{
	[ComVisible(true)]
	public class NativesApi
	{
		internal NativesApi() { }

		string GetCode(string libName)
		{
			var filePath = "Gel.lib." + libName + ".js";
			return ScriptEmbedded.ReadFile(filePath);
		}

		public bool hasOwnProperty(string id)
		{
			return typeof(NativesApi).GetProperty(id) != null;
		}

		public string util { get { return GetCode("util"); } }

		public string events { get { return GetCode("events"); } }

		public string stream { get { return GetCode("stream"); } }
	}
}
