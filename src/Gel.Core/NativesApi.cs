using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Gel.Scripting;

namespace Gel
{
	public class NativesApi
	{
		internal NativesApi() { }

		public bool exists(string id)
		{
			if (id.StartsWith("test."))
			{	// Gel.test.testName.js
				return ScriptSource.GetAssemblyForEmbedded("Gel.Core." + id + ".js") != null;
			}
			return ScriptSource.GetAssemblyForEmbedded(LibPath(id)) != null;
		}

		public string getSource(string id)
		{
			if (id.StartsWith("test."))
			{	// Gel.test.testName.js
				return ScriptEmbedded.ReadFile("Gel.Core." + id + ".js");
			}
			return ScriptEmbedded.ReadFile(LibPath(id));
		}

		static string LibPath(string id)
		{
			return "Gel.Core.lib." + id + ".js";
		}
	}
}
