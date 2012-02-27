using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gel.Scripting.JScript;
using Gel.Scripting;

namespace TestCore.Eval
{
	[TestClass]
	public class EvalTests : Base.JsEngineTests
	{
		[TestMethod]
		public void SimpleEvals()
		{
			var r = js.Eval("2 + 2;");
			Assert.IsNotNull(r);
			Assert.AreEqual("4", r.ToString());
		}
	}
}
