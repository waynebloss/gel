using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Gel.Scripting
{
	#region IScriptSource

	public interface IScriptSource
	{
		string Path { get; }
		int GetLineSource(int line, out IScriptSource source);
		System.IO.TextReader GetReader();
	}

	#endregion

	#region ScriptSource

	public static class ScriptSource
	{
		static ScriptSource()
		{
			_assemblyByEmbedPath = new Dictionary<string, Assembly>();
			_assemblyReg = new List<Assembly>();
			RegisterAssemblyForEmbedded(typeof(ScriptSource).Assembly);
		}

		public static IEnumerable<string> AsEnumerableLines(this IScriptSource source)
		{
			using (var rdr = source.GetReader())
			{
				string line = null;

				while ((line = rdr.ReadLine()) != null)
				{
					yield return line;
				}
			}
		}

		public static string ReadAll(this IScriptSource source)
		{
			using (var reader = source.GetReader())
				return reader.ReadToEnd();
		}

		#region Assembly for Embedded

		static readonly Dictionary<string, Assembly> _assemblyByEmbedPath;
		static readonly List<Assembly> _assemblyReg;

		internal static Assembly GetAssemblyForEmbedded(string path)
		{
			Assembly value;
			if (!_assemblyByEmbedPath.TryGetValue(path, out value))
				return null;

			return value;
		}

		public static void RegisterAssemblyForEmbedded(Assembly value)
		{
			/// Guard against double registration.
			if (_assemblyReg.Contains(value))
				return;
			/// Register the assembly.
			_assemblyReg.Add(value);
			/// Map all of the assembly's resource file paths to the assembly.
			/// Registrations added later override earlier registrations.
			foreach (var path in value.GetManifestResourceNames())
			{
				System.Diagnostics.Debug.Print("RegisterAssemblyForEmbedded assm: {0}, path: {1}.", value.GetName().Name, path);

				_assemblyByEmbedPath[path] = value;
			}
		}

		#endregion
	}

	#endregion

	#region ScriptEmbedded

	public class ScriptEmbedded : IScriptSource
	{
		readonly System.Reflection.Assembly _ass;
		readonly string _path;

		public ScriptEmbedded(string path)
			: this(path, ScriptSource.GetAssemblyForEmbedded(path))
		{
			
		}

		public ScriptEmbedded(string path, System.Reflection.Assembly ass)
		{
			_ass = ass;
			_path = path;
		}

		public string Path { get { return _path; } }

		public int GetLineSource(int line, out IScriptSource source)
		{
			source = this;
			return line;
		}

		public System.IO.TextReader GetReader()
		{
			return new System.IO.StreamReader(_ass.GetManifestResourceStream(_path));
		}

		/// <summary>
		/// Loads an Embedded Resource file as a string, from the _local_ assembly.
		/// </summary>
		/// <param name="path">Path to the embedded file resource, starting with the root
		/// namespace of the assembly, up to and including the file name.
		/// (e.g. RootNs.Folders.FileName1.html)</param>
		/// <returns></returns>
		public static string ReadFile(string path)
		{
			System.Diagnostics.Debug.Print("READING FILE: " + path);
			return ReadFile(typeof(ScriptEmbedded).Assembly, path);
		}
		/// <summary>
		/// Loads an Embedded Resource file as a string, from the given source assembly.
		/// </summary>
		/// <param name="source">The assembly from which to load the file.</param>
		/// <param name="path">Path to the embedded file resource, starting with the root
		/// namespace of the assembly, up to and including the file name.
		/// (e.g. RootNs.Folders.FileName1.html)</param>
		/// <returns></returns>
		public static string ReadFile(System.Reflection.Assembly source, string path)
		{
			var stream = source.GetManifestResourceStream(path);
			using (var reader = new System.IO.StreamReader(stream))
			{
				return reader.ReadToEnd();
			}
		}
	}

	#endregion

	#region ScriptFile

	public class ScriptFile : IScriptSource
	{
		readonly Encoding _encoding;
		readonly string _path;

		public ScriptFile(string path) : this(path, Encoding.UTF8) { }

		public ScriptFile(string path, Encoding encoding)
		{
			_encoding = encoding;
			_path = path;
		}

		public string Path { get { return _path; } }

		public int GetLineSource(int line, out IScriptSource source)
		{
			source = this;
			return line;
		}

		public System.IO.TextReader GetReader()
		{
			return new System.IO.StreamReader(_path, _encoding, true);
		}
	}

	#endregion

	#region ScriptCode

	public class ScriptCode : IScriptSource
	{
		readonly string _code;

		public ScriptCode(string code) : this(code, null) { }

		public ScriptCode(string code, string path)
		{
			_code = code;
			_Path = path;
		}

		readonly string _Path;
		public string Path { get { return _Path; } }

		public int GetLineSource(int line, out IScriptSource source)
		{
			source = this;
			return line;
		}

		public System.IO.TextReader GetReader()
		{
			return new System.IO.StringReader(_code);
		}
	}

	#endregion
}
