using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Gel.Scripting;

namespace Gel
{
	[ComVisible(true)]
	public class EvalsApi
	{
		internal EvalsApi() { }

		public void runInThisContext(string code, string fileName, bool displayError)
		{
			var src = new ScriptCode(code, fileName);
			App.Current.Script.Exec(src);
		}
	}
}
