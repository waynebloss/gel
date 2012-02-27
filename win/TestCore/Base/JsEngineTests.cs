using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gel.Scripting.JScript;
using Gel.Scripting;

namespace TestCore.Base
{
	[TestClass]
	public class JsEngineTests
	{
		protected TestContext Test;
		public TestContext TestContext { get { return Test; } set { Test = value; } }

		protected JsEngine js;

		[TestInitialize]
		public virtual void OnBeforeTest()
		{
			if (js != null)
				js.Dispose();

			js = new JsEngine();
		}
		
		[TestCleanup]
		public virtual void OnAfterTest()
		{
			var js = this.js; this.js = null;
			if (js != null)
				js.Dispose();
		}
	}
}
