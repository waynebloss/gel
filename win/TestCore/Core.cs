using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gel.Scripting.JScript;
using Gel.Scripting;

namespace TestCore
{
	[TestClass]
	public class Core
	{
		[AssemblyInitialize]
		public static void AssemblyInit(TestContext context)
		{
			ScriptSource.RegisterAssemblyForEmbedded(typeof(Core).Assembly);
		}
	}
}
