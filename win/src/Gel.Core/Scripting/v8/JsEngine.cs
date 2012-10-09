using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using Gel.IO;
using Noesis.Javascript;

namespace Gel.Scripting.v8
{
	public partial class JsEngine : IScriptEngine, IDisposable
	{
		JavascriptContext _engine;

		#region Constructor

		public JsEngine()
		{
			_engine = CreateNativeEngine();

			_sourceByName = new Dictionary<string, IScriptSource>();
		}

		static JavascriptContext CreateNativeEngine()
		{
			return new JavascriptContext();
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
				var eng = _engine; _engine = null;
				if (eng != null) eng.Dispose();
			}
			// Resources that may be released during dispose or finalize.
		}

		#endregion

		#region Named Items

		public JsEngine SetNamedItem(string name, object value)
		{
			return SetNamedItem(name, value, false);
		}

		public JsEngine SetNamedItem(string name, object value, bool withEvents)
		{
			if (name == null)
				throw new ArgumentNullException("name");

			_engine.SetParameter(name, value);

			return this;
		}

		#endregion

		#region Sources

		readonly Dictionary<string, IScriptSource> _sourceByName;

		void AddSource(IScriptSource source)
		{
			_sourceByName.Add(source.Path, source);
		}
		
		#endregion

		#region Exec

		public void Exec(string text)
		{
			Exec(text, "(global)");
		}

		public void Exec(IScriptSource source)
		{
			AddSource(source);
			Exec(source.ReadAll(), source.Path);
		}

		void Exec(string text, string resourceName)
		{
			if (text == null)
				throw new ArgumentNullException("text");

			try
			{
				_engine.Run(text, resourceName);
			}
			catch (JavascriptException ex)
			{
				Debug.Print("Error: {0}", ex.Message);
				foreach (var key in ex.Data.Keys)
				{
					Debug.Print("{0}: {1}", key, ex.Data[key]);
				}
				Debug.Print("--");
			}
		}

		#endregion

		#region Eval

		public object Eval(string expression)
		{
			return Eval(expression, "(global)");
		}

		public object Eval(IScriptSource source)
		{
			AddSource(source);
			return Eval(source.ReadAll(), source.Path);
		}

		object Eval(string expression, string resourceName)
		{
			if (expression == null)
				throw new ArgumentNullException("expression");

			return _engine.Run(expression, resourceName);
		}

		#endregion
	}
}