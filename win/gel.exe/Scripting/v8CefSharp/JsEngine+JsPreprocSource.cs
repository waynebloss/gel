using System;
using System.Collections.Generic;
//using System.Reflection;
using System.Text;

namespace Gel.Scripting.v8CefSharp
{
	partial class JsEngine
	{
		#region Preprocess Methods

		public static IScriptSource Preprocess(IScriptSource source)
		{
			return new JsPreprocSource(source);
		}
		
		public static IScriptSource PreprocessEmbedded(string path)
		{
			return Preprocess(new ScriptEmbedded(path));
		}

		public static IScriptSource PreprocessFile(string filePathName)
		{
			return Preprocess(new ScriptFile(filePathName));
		}

		#endregion

		class JsPreprocSource : IScriptSource
		{
			readonly IScriptSource _source;

			public JsPreprocSource(IScriptSource source)
			{
				_source = source;
			}

			public string Path { get { return _source.Path; } }

			public int GetLineSource(int line, out IScriptSource source)
			{
				foreach (var item in _lineSrcMap)
				{
					if (line >= item.First && line <= item.Last)
					{
						source = item.Source;
						return (line - item.First) + item.Offset;
					}
				}
				source = _source;
				return line;
			}

			public System.IO.TextReader GetReader()
			{
				var code = Process();

				return new System.IO.StringReader(code);
			}

			#region LineSource

			class LineSource
			{
				public LineSource(IScriptSource source)
					: this(source, 0) { }
				
				public LineSource(IScriptSource source, int offset)
				{
					_Source = source;
					First = -1;
					Last = -1;
					Offset = offset;
				}

				/// <summary>
				/// First line that includes the source.
				/// </summary>
				public int First { get; set; }

				/// <summary>
				/// Last line that includes the source.
				/// </summary>
				public int Last { get; set; }

				/// <summary>
				/// Offset line number of the source being represented.
				/// </summary>
				public int Offset { get; set; }

				readonly IScriptSource _Source;
				/// <summary>
				/// The source of the range of lines.
				/// </summary>
				public IScriptSource Source { get { return _Source; } }
			}

			#endregion

			#region Process

			const string IncLex = "// #include ";
			const int IncLexLen = 12;
			const int IncLexMinLen = 15;
			// Examples of minimally useful #include lexemes:
			//
			// 1) // #include <f>
			// 2) // #include "f"
			
			string _processedCode;
			List<LineSource> _lineSrcMap;

			string Process()
			{
				if (_processedCode != null)
					return _processedCode;

				var sb = new StringBuilder();
				var map = new List<LineSource>();
				int count = 0;

				Process(_source, sb, map, 0, ref count, null);
				_lineSrcMap = map;
				_processedCode = sb.ToString();

				return _processedCode;
			}

			static void Add(LineSource chunk, ref int count, ref int localCount)
			{
				if (chunk.First < 0)
					chunk.First = count;
				chunk.Last = count;

				count++;
				localCount++;
			}

			static void Process(IScriptSource source, StringBuilder sb, List<LineSource> map, int level, ref int count, string includeArg)
			{
				var chunk = new LineSource(source);
				var localCount = 0;

				var indent = (string)null;
				var lines = source.AsEnumerableLines();

				foreach (var line in lines)
				{
					/// Append lines that don't start with <see cref="IncLex"/>.
					/// 
					if (line.Length < IncLexMinLen ||
						!line.StartsWith(IncLex))
					{
						sb.AppendLine(line);
						Add(chunk, ref count, ref localCount);
						continue;
					}
					/// Get the argument portion of the include statement.
					/// If none can be found, just append the line.
					var argEnd = line.IndexOfAny(new[] { '>', '"' }, IncLexLen + 1);
					if (argEnd < IncLexLen)
					{
						sb.AppendLine(line);
						Add(chunk, ref count, ref localCount);
						continue;
					}
					var arg = line.Substring(IncLexLen + 1, argEnd - IncLexLen - 1);
					/// Get the argument delimiter, either an angle-bracket or
					/// a double-quote. An angle-bracket is an embedded file, 
					/// a double-quote is a regular file or possibly a uri in
					/// the future.
					var c = line[IncLexLen];

					switch (c)
					{
					case '<':
						/// Embedded Path.
						indent = indent ?? new String(' ', level);
						sb.AppendLine(String.Format("{0}// <{1}> include BEGIN", indent, arg));
						Add(chunk, ref count, ref localCount);

						map.Add(chunk);
						
						var efileSrc = new ScriptEmbedded(arg);
						Process(efileSrc, sb, map, level + 1, ref count, arg);

						// New chunk of source.
						chunk = new LineSource(source, localCount);

						break;
					case '"':
						/// TODO: Preprocess Include File Path.
						throw new NotImplementedException();
					default:
						sb.AppendLine(line);
						Add(chunk, ref count, ref localCount);
						break;
					}
				}
				if (level > 0)
				{
					indent = new String(' ', level - 1);
					sb.AppendLine(String.Format("{0}// <{1}> include END", indent, includeArg));
					Add(chunk, ref count, ref localCount);
				}
				map.Add(chunk);
			}

			#endregion
		}
	}
}
