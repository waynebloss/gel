using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace Gel
{
	[ComVisible(true)]
	public class FsApi
	{
		public bool isDirectory(string path)
		{
			Debug.Print("isDirectory: " + path);
			return Directory.Exists(path);
		}

		public bool fileExists(string path)
		{
			Debug.Print("fileExists: {0}, {1}", File.Exists(path), path);
			return File.Exists(path);
		}

		public bool pathExists(string path)
		{
			Debug.Print("pathExists: " + path);
			return File.Exists(path) || Directory.Exists(path);
		}

		public string readFileSync(string filename, string encoding)
		{
			Debug.Print("readFileSync: " + filename);
			Debug.Assert(encoding == "utf8", "Unsupported Encoding: '" + encoding + "'.");

			return File.ReadAllText(filename, Encoding.UTF8);
		}
	}
}
