using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using EXCEPINFO = System.Runtime.InteropServices.ComTypes.EXCEPINFO;

namespace Gel.Scripting.ActiveScript
{
	/// <summary>
	/// Wrapper for working with the <see cref="IActiveScriptParse32"/> or the
	/// <see cref="IActiveScriptParse64"/> interface of the given script engine.
	/// </summary>
	class ActiveScriptParser : IDisposable
	{
		IActiveScriptParse32 _p32;
		IActiveScriptParse64 _p64;
		readonly bool _is32bit;

		public ActiveScriptParser(object engine)
		{
			if (IntPtr.Size == 4)
			{
				_is32bit = true;
				_p32 = (IActiveScriptParse32)engine;
			}
			else
				_p64 = (IActiveScriptParse64)engine;
		}

		public bool Is32bit { get { return _is32bit; } }

		public void InitNew()
		{
			if (_p32 != null)
				_p32.InitNew();
			else
				_p64.InitNew();
		}

		public void AddScriptlet
		(
			string defaultName,
			string code,
			string itemName,
			string subItemName,
			string eventName,
			string delimiter,
			IntPtr sourceContextCookie,
			uint startingLineNumber,
			ScriptText flags,
			out string name,
			out EXCEPINFO exceptionInfo
		)
		{
			if (_p32 != null)
				_p32.AddScriptlet(defaultName, code, itemName, subItemName, eventName,
					delimiter, sourceContextCookie, startingLineNumber, flags, out name, out exceptionInfo);
			else
				_p64.AddScriptlet(defaultName, code, itemName, subItemName, eventName,
					delimiter, sourceContextCookie, startingLineNumber, flags, out name, out exceptionInfo);
		}

		public void ParseScriptText
		(
			string code,
			string itemName,
			[MarshalAs(UnmanagedType.IUnknown)]
			object context,
			string delimiter,
			IntPtr sourceContextCookie,
			uint startingLineNumber,
			ScriptText flags,
			[MarshalAs(UnmanagedType.Struct)]
			out object result,
			out EXCEPINFO exceptionInfo
		)
		{
			if (_p32 != null)
				_p32.ParseScriptText(code, itemName, context, delimiter, sourceContextCookie,
					startingLineNumber, flags, out result, out exceptionInfo);
			else
				_p64.ParseScriptText(code, itemName, context, delimiter, sourceContextCookie,
					startingLineNumber, flags, out result, out exceptionInfo);
		}

		#region IDisposable

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			//if (disposing)
			//{
			//	// Managed, disposable resources that should be released during dispose only.
			//}
			// Resources that may be released during dispose or finalize.

			var p32 = _p32; _p32 = null;
			ComUtil.DisposeComObject(p32, disposing);

			var p64 = _p64; _p64 = null;
			ComUtil.DisposeComObject(p64, disposing);
		}

		#endregion

		~ActiveScriptParser()
		{
			Dispose(false);
		}
	}
}
