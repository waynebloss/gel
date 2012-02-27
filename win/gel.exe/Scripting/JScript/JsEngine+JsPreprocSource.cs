using System;
using System.Collections.Generic;
//using System.Reflection;
using System.Text;

namespace Gel.Scripting.JScript
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
						return line - item.First + item.Offset + 1;
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
				{
					_Source = source;
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

				Process(_source, sb, map, 0, ref count);
				_lineSrcMap = map;
				_processedCode = sb.ToString();

				return _processedCode;
			}

			static void Process(IScriptSource source, StringBuilder sb, IList<LineSource> map, int level, ref int count)
			{
				var chunk = new LineSource(source);
				chunk.First = count + 1;
				chunk.Last = count;

				var indent = (string)null;
				var lines = source.AsEnumerableLines();

				foreach (var line in lines)
				{
					/// Append lines that don't start with <see cref="IncLex"/>.
					/// 
					if (line.Length < IncLexMinLen ||
						!line.StartsWith(IncLex))
					{
						chunk.Last++;
						count++;
						sb.AppendLine(line);
						continue;
					}
					/// Get the argument portion of the include statement.
					/// If none can be found, just append the line.
					var argEnd = line.IndexOfAny(new[] { '>', '"' }, IncLexLen + 1);
					if (argEnd < IncLexLen)
					{
						chunk.Last++;
						count++;
						sb.AppendLine(line);
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
						chunk.Last++;
						count++;
						sb.AppendLine(String.Format("{0}// <{1}> include BEGIN", indent, arg));

						map.Add(chunk);

						// New chunk of source.
						int offset = count - chunk.First + 1;
						chunk = new LineSource(source);
						chunk.Offset = offset;
						
						var efileSrc = new ScriptEmbedded(arg);
						Process(efileSrc, sb, map, level + 1, ref count);
						sb.AppendLine(String.Format("{0}// <{1}> include END", indent, arg));

						chunk.First = count + 1;
						chunk.Last = count;

						break;
					case '"':
						/// TODO: Preprocess Include File Path.
						throw new NotImplementedException();
					default:
						chunk.Last++;
						count++;
						sb.AppendLine(line);
						break;
					}
				}
				if (level > 0)
				{
					chunk.Last++;
					count++;
				}
				map.Add(chunk);
			}

			#endregion
		}
	}
}
