using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gel.Scripting.v8;
using Gel.Scripting;
using System.Diagnostics;

namespace TestCore.Preproc
{
	[TestClass]
	public class PreprocTests : Base.JsEngineTests
	{
		[TestMethod]
		public void SimplePreprocAB()
		{
			var rootSrc = JsEngine.PreprocessEmbedded("TestCore.Preproc.a.js");

			var code = rootSrc.ReadAll();

			Trace.WriteLine(code);

			Assert.AreEqual(@"a1
a2
a3
// <TestCore.Preproc.b.js> include BEGIN
b1
b2
b3
b4
b5
// <TestCore.Preproc.b.js> include END
a5
a6
a7
a8
a9
a10
", code);
			Test.WriteLine("Line, Source, Source Line");

			var src = (IScriptSource)null;
			var line = 0;
			/// Test lines 1 to 4.
			for (int i = 1; i < 5; i++)
			{
				line = rootSrc.GetLineSource(i, out src);
				Test.WriteLine("{0}, {1}, {2}", i, GetFN(src.Path), line);
				Assert.AreEqual(i, line);
				Assert.AreEqual("TestCore.Preproc.a.js", src.Path, "i: " + i.ToString());
			}
			/// Test lines 5 to 10.
			for (int i = 5; i < 11; i++)
			{
				line = rootSrc.GetLineSource(i, out src);
				Test.WriteLine("{0}, {1}, {2}", i, GetFN(src.Path), line);
				Assert.AreEqual(i - 4, line);
				Assert.AreEqual("TestCore.Preproc.b.js", src.Path, "i: " + i.ToString());
			}
			/// Test lines 11 to 16.
			for (int i = 11; i < 17; i++)
			{
				line = rootSrc.GetLineSource(i, out src);
				Test.WriteLine("{0}, {1}, {2}", i, GetFN(src.Path), line);
				Assert.AreEqual(i - 6, line);
				Assert.AreEqual("TestCore.Preproc.a.js", src.Path, "i: " + i.ToString());
			}
		}

		[TestMethod]
		public void SimplePreprocCAB()
		{
			var rootSrc = JsEngine.PreprocessEmbedded("TestCore.Preproc.c.js");

			var code = rootSrc.ReadAll();

			Trace.WriteLine(code);

			Test.WriteLine("Line, Source, Source Line");

			var src = (IScriptSource)null;
			var line = 0;
			/// Test lines 1 to 3.		c.js
			for (int i = 1; i < 4; i++)
			{
				line = rootSrc.GetLineSource(i, out src);
				Test.WriteLine("{0}, {1}, {2}", i, GetFN(src.Path), line);
				Assert.AreEqual(i, line, "i: " + i.ToString());
				Assert.AreEqual("TestCore.Preproc.c.js", src.Path, "i: " + i.ToString());
			}
			/// Test lines 4 to 7.		a.js
			for (int i = 4; i < 8; i++)
			{
				line = rootSrc.GetLineSource(i, out src);
				Test.WriteLine("{0}, {1}, {2}", i, GetFN(src.Path), line);
				Assert.AreEqual(i - 3, line, "i: " + i.ToString());
				Assert.AreEqual("TestCore.Preproc.a.js", src.Path, "i: " + i.ToString());
			}
			/// Test lines 8 to 13.		b.js
			for (int i = 8; i < 14; i++)
			{
				line = rootSrc.GetLineSource(i, out src);
				Test.WriteLine("{0}, {1}, {2}", i, GetFN(src.Path), line);
				Assert.AreEqual(i - 7, line, "i: " + i.ToString());
				Assert.AreEqual("TestCore.Preproc.b.js", src.Path, "i: " + i.ToString());
			}
			/// Test lines 14 to 20.	a.js
			for (int i = 14; i < 21; i++)
			{
				line = rootSrc.GetLineSource(i, out src);
				Test.WriteLine("{0}, {1}, {2}", i, GetFN(src.Path), line);
				Assert.AreEqual(i - 9, line, "i: " + i.ToString());
				Assert.AreEqual("TestCore.Preproc.a.js", src.Path, "i: " + i.ToString());
			}
			/// Test lines 21 to 23.	c.js
			for (int i = 21; i < 24; i++)
			{
				line = rootSrc.GetLineSource(i, out src);
				Test.WriteLine("{0}, {1}, {2}", i, GetFN(src.Path), line);
				Assert.AreEqual(i - 17, line, "i: " + i.ToString());
				Assert.AreEqual("TestCore.Preproc.c.js", src.Path, "i: " + i.ToString());
			}
		}

		[TestMethod]
		public void ErrPreprocAB()
		{
			var rootSrc = JsEngine.PreprocessEmbedded("TestCore.Preproc.errA.js");

			var code = rootSrc.ReadAll();

			Trace.WriteLine(code);

			try
			{
				js.Exec(rootSrc);
			}
			catch (Exception ex)
			{
				Test.WriteLine(ex.Message);
				Assert.AreEqual(
					@"Microsoft JScript runtime error - Error: -2146823281 (0x800A138F): " +
					@"The value of the property 'NotAFunction' is null or undefined, not " +
					@"a Function object at line 4, column 1 in TestCore.Preproc.errA.js.",
					ex.Message);
			}
		}

		[TestMethod]
		public void ErrPreprocCAB()
		{
			var rootSrc = JsEngine.PreprocessEmbedded("TestCore.Preproc.errC.js");

			var code = rootSrc.ReadAll();

			Trace.WriteLine(code);

			try
			{
				js.Exec(rootSrc);
			}
			catch (Exception ex)
			{
				Test.WriteLine(ex.Message);
				Assert.AreEqual(
					@"Microsoft JScript compilation error - Error: -2146827281 (0x800A03EF): " +
					@"Expected ']' at line 2, column 15 in TestCore.Preproc.errC.js. Source " +
					@"line: 'Syn[taxError();'.",
					ex.Message);
			}
		}

		static string GetFN(string path)
		{
			return @"""" + path.Replace("Gel.test.includes.", "").Replace(".js", "") + @"""";
		}
	}
}
