using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Gel.Scripting;

namespace Gel
{
	public class EvalsApi
	{
		internal EvalsApi() { }

		public object runInThisContext(string code, string fileName, bool displayError)
		{
			var src = new ScriptCode(code, fileName);
			return App.Current.Script.Eval(src);
		}
	}
}
