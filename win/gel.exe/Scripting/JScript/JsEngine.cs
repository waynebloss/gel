using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using Gel.Scripting.ActiveScript;
using EXCEPINFO = System.Runtime.InteropServices.ComTypes.EXCEPINFO;
using System.Diagnostics;
using Gel.IO;

namespace Gel.Scripting.JScript
{
	public partial class JsEngine : ActiveScriptSite, IScriptEngine, IDisposable
	{
		IActiveScript _engine;
		ActiveScriptParser _parser;

		#region Constructor

		static readonly Type ChakraEngineType = Type.GetTypeFromCLSID(new Guid("16d51579-a30b-4c8b-a276-0ff4dc41e755"), false);
		static readonly Type JscriptEngineType = Type.GetTypeFromProgID("jscript", false);

		public JsEngine()
			: this(DefaultExceptionHandler) { }

		public JsEngine(UnhandledExceptionEventHandler uexHandler)
		{
			//DocVersionString = "DOCTYPE html";

			_engine = CreateNativeEngine();
			_engine.SetScriptSite(this);

			// Set properties.
			var activeScriptProperty = (IActiveScriptProperty)_engine;

			// Indicate that we are not combining multiple script engines.
			//object value = true;
			//activeScriptProperty.SetProperty(ScriptProp.ABBREVIATE_GLOBALNAME_RESOLUTION, IntPtr.Zero, ref value);

			// Upgrade the version of the script engine to 5.8 (IE 8).
			object value = (int)ScriptLangVer.V9;
			activeScriptProperty.SetProperty(ScriptProp.INVOKEVERSIONING, IntPtr.Zero, ref value);

			_parser = new ActiveScriptParser(_engine);
			_parser.InitNew();
			_uexHandler = uexHandler;

			_namedItems = new Dictionary<string, object>();
			_sourceById = new Dictionary<int, IScriptSource>();
		}

		static IActiveScript CreateNativeEngine()
		{
			IActiveScript eng = null;
			if (ChakraEngineType != null)
			{
				eng = Activator.CreateInstance(ChakraEngineType) as IActiveScript;
				if (eng == null && JscriptEngineType != null)
					eng = Activator.CreateInstance(JscriptEngineType) as IActiveScript;
			}
			if (eng == null)
				throw new InvalidComObjectException("Jscript engine not found.");
			return eng;
		}

		#endregion

		#region IDisposable

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Managed, disposable resources that should be released during dispose only.
				var p = _parser; _parser = null;
				if (p != null)
					p.Dispose();
			}
			// Resources that may be released during dispose or finalize.

			var eng = _engine; _engine = null;
			ComUtil.DisposeComObject(eng, disposing);
		}

		#endregion

		~JsEngine()
		{
			Dispose(false);
		}

		#region Errors

		UnhandledExceptionEventHandler _uexHandler;

		static void DefaultExceptionHandler(object source, UnhandledExceptionEventArgs e)
		{
			var prefix = (e.IsTerminating ? "Fatal " : "") + "Exception Occurred. ";

			var ex = e.ExceptionObject as Exception;
			if (ex != null && !String.IsNullOrEmpty(ex.Message))
				Trace.TraceError(prefix + ex.Message);
			else
				Trace.TraceError(prefix + "Unknown Error.");
		}

		delegate bool ErrorCheckDelegate(ref int col, ref string text);

		protected override int GetLineSource(int sourceId, int sourceLine, ref int sourceCol, ref string sourceText, ref string path)
		{
			if (sourceId == 0)
			{
				path = "(global)";
				return sourceLine;
			}
			IScriptSource src;
			if (!_sourceById.TryGetValue(sourceId, out src))
			{
				return sourceLine;
			}
			IScriptSource src2;
			int srcLine = src.GetLineSource(sourceLine, out src2);
			if (src2 != null && src2.Path != null)
				path = src2.Path;
			return srcLine;
		}

		#endregion

		#region Named Items

		Dictionary<string, object> _namedItems;

		protected override object GetNamedItemObject(string name)
		{
			object value;
			if (!_namedItems.TryGetValue(name, out value))
				return null;
			else
				return value;
		}

		public JsEngine SetNamedItem(string name, object value)
		{
			return SetNamedItem(name, value, false);
		}

		public JsEngine SetNamedItem(string name, object value, bool withEvents)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			var flags = ScriptItem.IsVisible;
			if (withEvents)
				flags |= ScriptItem.IsSource;

			object oldValue;
			bool hasOldValue = _namedItems.TryGetValue(name, out oldValue);
			if (hasOldValue)
				_namedItems[name] = value;
			else
				_namedItems.Add(name, value);

			try
			{
				_engine.AddNamedItem(name, flags);
			}
			catch
			{
				if (hasOldValue)
					_namedItems[name] = oldValue;
				else
					_namedItems.Remove(name);

				throw;
			}
			return this;
		}

		#endregion

		#region Sources

		int _sourceId;

		readonly Dictionary<int, IScriptSource> _sourceById;

		int AddSource(IScriptSource source)
		{
			int id = ++_sourceId;
			_sourceById.Add(id, source);
			return id;
		}
		
		#endregion

		#region Exec

		public void Exec(string text)
		{
			Exec(text, IntPtr.Zero);
		}

		public void Exec(IScriptSource source)
		{
			var id = AddSource(source);
			Exec(source.ReadAll(), new IntPtr(id));
		}

		void Exec(string text, IntPtr sourceIdPtr)
		{
			if (text == null)
				throw new ArgumentNullException("text");

			OnBeforeParse();

			EXCEPINFO exInfo;
			object result;
			try
			{
				_parser.ParseScriptText(text, null, null, null, sourceIdPtr, 0, ScriptText.None, out result, out exInfo);
			}
			catch
			{
				var ex = ExtractSiteException();
				if (ex != null)
					throw ex;

				throw;
			}
		}

		#endregion

		#region Eval

		/// <summary>
		/// Name of the variable used to retrieve Eval values under certain conditions.
		/// </summary>
		internal const string EvalHackVarName = "__eval";

		public object Eval(string expression)
		{
			return Eval(expression, IntPtr.Zero);
		}

		public object Eval(IScriptSource source)
		{
			var id = AddSource(source);
			return Eval(source.ReadAll(), new IntPtr(id));
		}

		object Eval(string expression, IntPtr sourceIdPtr)
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			OnBeforeParse();

			EXCEPINFO exInfo;
			object result;
			try
			{
				if (_parser.Is32bit)
				{
					// TODO: Check that this Hack to get return value works as expected.
					expression = EvalHackVarName + "=" + expression;
				}
				_parser.ParseScriptText(expression, null, null, null, sourceIdPtr, 0, ScriptText.IsExpression, out result, out exInfo);
			}
			catch
			{
				var ex = ExtractSiteException();
				if (ex != null)
				{
					// TODO: Test the following hack to see if it corrects the error column number properly.
					if (ex.Line == 1) EvalHackErrorFixup(ex);
					throw ex;
				}

				throw;
			}
			if (!_parser.Is32bit)
				return result;

			// Hack to get return value.
			IntPtr dispatchPtr;
			_engine.GetScriptDispatch(null, out dispatchPtr);
			var dispatch = Marshal.GetObjectForIUnknown(dispatchPtr);
			try
			{
				return dispatch.GetType().InvokeMember(EvalHackVarName, BindingFlags.GetProperty, null, dispatch, null);
			}
			catch
			{
				var ex = ExtractSiteException();
				if (ex != null)
					throw ex;

				throw;
			}
		}

		static void EvalHackErrorFixup(ScriptException ex)
		{
			if (ex.Column > EvalHackVarName.Length)
				ex.Column -= EvalHackVarName.Length;

			
		}

		#endregion

		#region Parse

		void OnBeforeParse()
		{
			if (CanConnect)
				_engine.SetScriptState(ScriptState.Connected);
		}

		public ActiveScriptDispatch Parse(string text)
		{
			return Parse(text, IntPtr.Zero);
		}

		public ActiveScriptDispatch Parse(IScriptSource source)
		{
			var id = AddSource(source);
			return Parse(source.ReadAll(), new IntPtr(id));
		}

		ActiveScriptDispatch Parse(string text, IntPtr sourceIdPtr)
		{
			if (text == null)
				throw new ArgumentNullException("text");

			OnBeforeParse();

			EXCEPINFO exInfo;
			object result;
			try
			{
				_parser.ParseScriptText(text, null, null, null, sourceIdPtr, 0, ScriptText.IsPersistent, out result, out exInfo);
			}
			catch
			{
				var ex = ExtractSiteException();
				if (ex != null)
					throw ex;

				throw;
			}
			IntPtr scriptPtr;
			_engine.GetScriptDispatch(null, out scriptPtr);
			var parsed = new ActiveScriptDispatch(this, scriptPtr);
			return parsed;
		}

		#endregion
	}
}