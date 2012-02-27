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

		static Dictionary<string, Assembly> _assemblyByEmbedPath;

		internal static Assembly GetAssemblyForEmbedded(string path)
		{
			if (_assemblyByEmbedPath == null)
				return typeof(ScriptSource).Assembly;

			Assembly value;
			if (!_assemblyByEmbedPath.TryGetValue(path, out value))
				return typeof(ScriptSource).Assembly;

			return value;
		}

		public static void RegisterAssemblyForEmbedded(Assembly assembly)
		{
			if (_assemblyByEmbedPath == null)
			{
				/// Guard against registration of this assembly,
				/// which will be done automatically below.
				if (assembly == typeof(ScriptSource).Assembly)
					return;
				_assemblyByEmbedPath = new Dictionary<string, Assembly>();
				/// This assembly is always the first to be registered.
				RegisterAssemblyForEmbedded(typeof(ScriptSource).Assembly);
			}
			/// Guard against double registration.
			if (_assemblyByEmbedPath.Values.Contains(assembly))
				return;
			/// Map all of the assembly's resource file paths to the assembly.
			/// Registrations added later override earlier registrations.
			foreach (var path in assembly.GetManifestResourceNames())
			{
				System.Diagnostics.Debug.Print("Registering embedded file {0} to assembly {1}.", path, assembly.GetName().Name);

				_assemblyByEmbedPath[path] = assembly;
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

		public ScriptCode(string code)
		{
			_code = code;
		}

		public string Path { get { return null; } }

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
