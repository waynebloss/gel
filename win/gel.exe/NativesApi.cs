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

		public string path { get { return GetCode("path"); } }

		public string string_decoder { get { return GetCode("string_decoder"); } }

		public string assert { get { return GetCode("assert"); } }

		public string test_assert { get { return GetCode("test_assert"); } }

		public string timers { get { return GetCode("timers"); } }

		public string _linklist { get { return GetCode("_linklist"); } }
	}
}
