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
		public bool IsDirectory(string path)
		{
			return Directory.Exists(path);
		}

		public string readFileSync(string filename, string encoding)
		{
			Debug.Assert(encoding == "utf8", "Unsupported Encoding: '" + encoding + "'.");

			return
@"exports.exec = function()
{
	console.log('Hello from " + filename + @"');
};";
		}
	}
}
