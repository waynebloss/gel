using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Gel.IO
{
	public sealed class TempFile : IDisposable
	{
		public TempFile() :
			this(Path.GetTempPath()) { }

		public TempFile(string directory)
		{
			Create(Path.Combine(directory, Path.GetRandomFileName()));
		}

		~TempFile()
		{
			Delete();
		}

		public void Dispose()
		{
			Delete();
			GC.SuppressFinalize(this);
		}

		public string FilePath { get; private set; }

		private void Create(string path)
		{
			FilePath = path;
			using (File.Create(FilePath)) { };
		}

		private void Delete()
		{
			if (FilePath == null || !File.Exists(FilePath)) return;
			File.Delete(FilePath);
			FilePath = null;
		}
	}
}
