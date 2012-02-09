using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gel.Data;
using Gel;

namespace TestCore.Data
{
	[TestClass]
	public class JsonTests
	{
		TestContext Test;
		public TestContext TestContext { get { return Test; } set { Test = value; } }

		[TestMethod]
		public void JsonBasic()
		{
			var o = Json.Read("");
			Assert.IsNull(o);

			o = Json.Read("Hello");
			Assert.IsNull(o);

			o = Json.Read("\"Hello\"");
			Assert.IsNotNull(o);
			Assert.IsInstanceOfType(o, typeof(string));
			Assert.AreEqual("Hello", o);

			o = Json.Read("\"\\u00f1\"");
			Assert.IsNotNull(o);
			Assert.IsInstanceOfType(o, typeof(string));
			Assert.AreEqual("ñ", o);

			o = Json.Read("1234");
			Assert.IsNotNull(o);
			Assert.IsInstanceOfType(o, typeof(string));
			Assert.AreEqual(1234.ToString(), o);

			o = Json.Read("true");
			Assert.IsNotNull(o);
			Assert.AreEqual(true.ToString(), o);

			o = Json.Read("false");
			Assert.IsNotNull(o);
			Assert.AreEqual(false.ToString(), o);

			o = Json.Read("[0, 1, 2, 3]");
			Assert.IsNotNull(o);
			Assert.IsInstanceOfType(o, typeof(GelArray));

			var arr = (GelArray)o;
			var i = 0;

			foreach (var item in arr)
			{
				Assert.IsInstanceOfType(item, typeof(string));
				int n;
				Assert.IsTrue(int.TryParse((string)item, out n));
				Assert.AreEqual(i++, n);
			}
			Assert.AreEqual(i, 4);

			o = Json.Read(@"{""id"": ""test""}");
			Assert.IsNotNull(o);
			Assert.IsInstanceOfType(o, typeof(GelObject));

			var obj = (GelObject)o;
			object o2;

			Assert.IsTrue(obj.TryGetValue("id", out o2));
			Assert.IsInstanceOfType(o2, typeof(string));
			var str = (string)o2;
			Assert.AreEqual(str, "test");

			o = Json.Read(@"{""id"": ""test"", ""items"": [0, 1, 2, 3]}");

			Assert.IsNotNull(o);
			Assert.IsInstanceOfType(o, typeof(GelObject));
			obj = (GelObject)o;

			Assert.IsTrue(obj.TryGetValue("id", out o2));
			Assert.IsInstanceOfType(o2, typeof(string));
			str = (string)o2;
			Assert.AreEqual(str, "test");

			Assert.IsTrue(obj.TryGetValue("items", out o2));
			Assert.IsInstanceOfType(o2, typeof(GelArray));

			arr = (GelArray)o2;
			i = 0;

			foreach (var item in arr)
			{
				Assert.IsInstanceOfType(item, typeof(string));
				int n;
				Assert.IsTrue(int.TryParse((string)item, out n));
				Assert.AreEqual(i++, n);
			}
			Assert.AreEqual(i, 4);
		}

		[TestMethod]
		public void JsonDebugDump()
		{
			Json.DebugDump(
@"{	""type"": ""Window"",
	""text"": ""gelEd"",
	""icon"": ""gelEd-16sq.png"",
	""startPosition"": ""centerScreen"",

	""components"": [
		{""type"": ""MenuBar"",
			""items"": [
				{""id"": ""file"", ""text"": ""&File"",
					""items"": [
						{""id"": ""file-exit"", ""text"": ""E&xit""}
					]
				},
				{""id"": ""help"", ""text"": ""&Help"",
					""items"": [
						{""id"": ""help-about"", ""text"": ""&About""}
					]
				},
			]
		},
		{""type"": ""ToolBar"", 
			""items"": [
				{""id"": ""file-exit"", ""text"": ""E&xit""}
			]
		},
		{""type"": ""WebView"", ""url"": ""http://www.google.com/""},
		{""type"": ""StatusBar""}
	]
}"
			);
		}

		[TestMethod]
		public void JsonErrors()
		{
			var o = Json.Read(null);
			Assert.IsNull(o);

			var jr = new JsonReader<GelArray, GelObject>(null);
			o = jr.Read();
			Assert.IsNull(o);
			Assert.IsTrue(jr.HasError);
			Assert.IsTrue(jr.IsErrorEOF);
			Assert.IsTrue(jr.ErrorIndex == -1);

			jr = new JsonReader<GelArray, GelObject>("{testing: 1, 2, 3}");
			o = jr.Read();
			Assert.IsTrue(jr.HasError);
			Assert.IsTrue(jr.ErrorIndex == 1);
		}
	}
}
