using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using Gel.Scripting.ActiveScript;
using EXCEPINFO = System.Runtime.InteropServices.ComTypes.EXCEPINFO;
using System.Diagnostics;

namespace Gel.Scripting.ActiveScript
{
	public abstract class ActiveScriptSite : IActiveScriptSite
	{
		protected ActiveScriptSite()
		{
			DocVersionString = DateTime.UtcNow.ToString("o");
		}

		#region IActiveScriptSite

		void IActiveScriptSite.GetLCID(out int lcid)
		{
			lcid = Thread.CurrentThread.CurrentCulture.LCID;
		}

		#region GetItemInfo

		protected Exception ElementNotFoundException()
		{
			const int TYPE_E_ELEMENTNOTFOUND = unchecked((int)(0x8002802B));

			return new COMException(null, TYPE_E_ELEMENTNOTFOUND);
		}

		void IActiveScriptSite.GetItemInfo(string name, ScriptInfo returnMask, object[] item, IntPtr[] typeInfo)
		{
			DebugEvent("GetItemInfo");

			if ((returnMask & ScriptInfo.ITypeInfo) == ScriptInfo.ITypeInfo)
				throw new NotImplementedException();

			//object value;
			//if (!_itemInfo.TryGetValue(name, out value))
			//{
			//    /// Instead of throwing a COM exception here, which breaks
			//    /// <see cref="JsEngine.AddContext"/>, return something.
			//    /// 
			//    /// Returning String.Empty seems to cause a named object
			//    /// with no properties or methods (with typeof "object")
			//    /// to be created in JScript.
			//    /// 
			//    /// So, an exception would happen in JScript if anything
			//    /// tried to use it. The object that gets created CAN be
			//    /// deleted from within Jscript at some point though as well.
			//    /// 
			//    //throw new COMException(null, TYPE_E_ELEMENTNOTFOUND);
			//    item[0] = String.Empty;
			//}
			//else
			//{
			//    item[0] = value;
			//}

			item[0] = GetNamedItemObject(name);

			/// 
			/// With alternate declaration of <see cref="IActiveScriptSite.GetItemInfo"/>:
			///	<code>
			///	item = Marshal.GetIUnknownForObject(value);
			///	</code>
			/// (See <see cref="IActiveScriptSite.GetItemInfo"/> remarks).
			/// 
		}

		protected abstract object GetNamedItemObject(string name);

		#endregion

		#region GetDocVersionString

		protected string DocVersionString { get; set; }

		void IActiveScriptSite.GetDocVersionString(out string version)
		{
			version = DocVersionString;
		}

		#endregion

		#region OnScriptError

		ScriptException _siteException;

		protected bool SiteHasException { get { return _siteException != null; } }

		internal protected ScriptException ExtractSiteException()
		{
			var ex = _siteException;
			_siteException = null;
			return ex;
		}

		protected virtual int GetLineSource(int sourceId, int sourceLine, ref int sourceCol, ref string sourceText, ref string path)
		{
			return sourceLine;
		}

		void IActiveScriptSite.OnScriptError(IActiveScriptError scriptError)
		{
			DebugEvent("OnScriptError:");

			EXCEPINFO exInfo;
			scriptError.GetExceptionInfo(out exInfo);

			uint errCtx;
			uint errLine;
			int errCol;
			scriptError.GetSourcePosition(out errCtx, out errLine, out errCol);

			var errNum = exInfo.scode;
			var errLineText = SafeGetSourceLineText(scriptError, errNum);
			var errObjName = exInfo.bstrSource;
			var errDesc = exInfo.bstrDescription;

			string errFilePath = null;
			errLine = (uint)GetLineSource((int)errCtx, (int)errLine, ref errCol, ref errLineText, ref errFilePath);
			errFilePath = errFilePath ?? "[unknown]";

			string message = (!string.IsNullOrEmpty(errLineText)) ?
				"{1} - Error: {0} (0x{0:X8}): {2} at line {3}, column {4} in {5}. Source line: '{6}'." :
				"{1} - Error: {0} (0x{0:X8}): {2} at line {3}, column {4} in {5}.";

			message = string.Format(message,
				errNum, errObjName, errDesc,
				errLine, errCol, errFilePath,
				errLineText);

			_siteException = new ScriptException(message);
			_siteException.Column = errCol;
			_siteException.Description = errDesc;
			_siteException.Line = (int)errLine;
			_siteException.Number = errNum;
			_siteException.Text = errLineText ?? errObjName;

			System.Diagnostics.Debug.Print("\t" + _siteException.Message);
		}

		static string SafeGetSourceLineText(IActiveScriptError scriptError, int errNum)
		{
			const int MASK_SYNTAX_ERR = 0x1000;
			if ((MASK_SYNTAX_ERR & errNum) != 0)
				return null;

			string errLineText;
			try
			{
				scriptError.GetSourceLineText(out errLineText);
			}
			catch
			{
				errLineText = null;
				Debug.Print("Error calling IActiveScriptError.GetSourceLineText.");
			}
			return errLineText;
		}

		#endregion

		#region Script State

		ScriptState _state;

		protected bool CanConnect
		{
			get
			{
				return _state != ScriptState.Connected;
			}
		}

		void IActiveScriptSite.OnScriptTerminate(object result, EXCEPINFO exceptionInfo)
		{
			DebugEvent("OnScriptTerminate (result: {0})", result);
		}

		void IActiveScriptSite.OnStateChange(ScriptState scriptState)
		{
			_state = scriptState;
			DebugEvent("OnStateChange (scriptState: {0})", scriptState);
		}

		void IActiveScriptSite.OnEnterScript()
		{
			DebugEvent("OnEnterScript");
		}

		void IActiveScriptSite.OnLeaveScript()
		{
			DebugEvent("OnLeaveScript");
		}

		#endregion

		#endregion

		[Conditional("DEBUG")]
		protected void DebugEvent(string eventHeader, params object[] args)
		{
			//Debug.Print(GetType().Name + "." + String.Format(eventHeader, args));
		}
	}
}
