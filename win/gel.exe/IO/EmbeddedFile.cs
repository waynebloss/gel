using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Gel.Scripting;

namespace Gel.IO
{
	static class EmbeddedFile
	{
		public static string[] GetNames(string path, string extension)
		{
			return GetNames(typeof(EmbeddedFile).Assembly, path, extension);
		}

		public static string[] GetNames(Assembly source, string path, string extension)
		{
			return source.GetManifestResourceNames()
				.Where(name => name.StartsWith(path) && name.EndsWith(extension))
				.ToArray();
		}
	}
}
