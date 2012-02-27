using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gel.Scripting
{
	public sealed class ScriptContext
	{
		public static readonly ScriptContext Global = new ScriptContext(0, "Global");

		public ScriptContext(int id, string name)
		{
			_Id = id;
			_Name = name;
		}

		readonly int _Id;
		public int Id { get { return _Id; } }

		readonly string _Name;
		public string Name { get { return _Name; } }
	}
}
