using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;

namespace Gel.Scripting.ActiveScript
{
	/// <summary>
	/// Defines a pre-parsed script object that can be evaluated at runtime.
	/// </summary>
	public class ActiveScriptDispatch : IDisposable
	{
		readonly ActiveScriptSite _engine;
		object _script;

		internal ActiveScriptDispatch(ActiveScriptSite engine, IntPtr scriptPtr)
		{
			_engine = engine;
			_script = Marshal.GetObjectForIUnknown(scriptPtr);
		}

		#region Dispose

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
			}

			var disp = _script; _script = null;
			ComUtil.DisposeComObject(disp, disposing);
		}

		#endregion

		~ActiveScriptDispatch()
		{
			Dispose(false);
		}

		#region Introspection

		Dictionary<string, int> _funcs;
		List<string> _props;

		public IDictionary<string, int> GetMethods()
		{
			if (_funcs == null)
				Introspect();

			return new Dictionary<string, int>(_funcs);
		}

		public IList<string> GetProperties()
		{
			if (_props == null)
				Introspect();

			return new System.Collections.ObjectModel.ReadOnlyCollection<string>(_props);
		}

		static void GetMembers(
			System.Runtime.InteropServices.ComTypes.ITypeInfo typeInfo,
			System.Runtime.InteropServices.ComTypes.TYPEATTR typeAttr,
			out Dictionary<string, int> methodParamCountLookup,
			out List<string> propertyNames
		)
		{
			var methods = new Dictionary<string,int>();
			var props = new List<string>();

			for (int i = 0; i < typeAttr.cFuncs; i++)
			{
				IntPtr pFuncDesc = IntPtr.Zero;
				System.Runtime.InteropServices.ComTypes.FUNCDESC funcDesc;
				string strName, strDocString, strHelpFile;
				int dwHelpContext;

				typeInfo.GetFuncDesc(i, out pFuncDesc);
				funcDesc =
					(System.Runtime.InteropServices.ComTypes.FUNCDESC)
					Marshal.PtrToStructure(pFuncDesc,
						typeof(System.Runtime.InteropServices.ComTypes.FUNCDESC));

				switch (funcDesc.invkind)
				{
				case System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_FUNC:
				case System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYGET:

					typeInfo.GetDocumentation(funcDesc.memid, out strName, out strDocString, out dwHelpContext, out strHelpFile);

					switch (funcDesc.invkind)
					{
					case System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_FUNC:
						methods.Add(strName, funcDesc.cParams);
						break;
					case System.Runtime.InteropServices.ComTypes.INVOKEKIND.INVOKE_PROPERTYGET:
						props.Add(strName);
						break;
					}
					break;
				}
			}
			methodParamCountLookup = methods;
			propertyNames = props;
		}

		void Introspect()
		{
			IntPtr pTypeAttr = IntPtr.Zero;
			System.Runtime.InteropServices.ComTypes.ITypeInfo typeInfo;

			var disp = _script as IDispatch;

			typeInfo = disp.GetTypeInfo(0, 0);
			typeInfo.GetTypeAttr(out pTypeAttr);
			try
			{
				var typeAttr =
					(System.Runtime.InteropServices.ComTypes.TYPEATTR)
					 Marshal.PtrToStructure(pTypeAttr,
						typeof(System.Runtime.InteropServices.ComTypes.TYPEATTR));

				GetMembers(typeInfo, typeAttr, out _funcs, out _props);

				/// Consider the following, if getting properties from the
				/// IDispatch isn't working. -wdb
				/// 
				//foreach (var item in _script.GetType().GetProperties(
				//	BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
				//)
				//{
				//    _props.Add(item.Name);
				//}

			}
			finally
			{
				if (typeInfo != null)
					typeInfo.ReleaseTypeAttr(pTypeAttr);
			}
		}

		#endregion

		#region Late Binding

		public object CallMethod(string methodName, params object[] arguments)
		{
			if (_script == null)
				throw new InvalidOperationException();

			if (methodName == null)
				throw new ArgumentNullException("methodName");

			try
			{
				return _script.GetType().InvokeMember(methodName, BindingFlags.InvokeMethod, null, _script, arguments);
			}
			catch
			{
				var ex = _engine.ExtractSiteException();
				if (ex != null)
					throw ex;

				throw;
			}
		}

		public object GetValue(string propName)
		{
			if (_script == null)
				throw new InvalidOperationException();

			if (propName == null)
				throw new ArgumentNullException("propName");

			try
			{
				return _script.GetType().InvokeMember(propName, BindingFlags.GetProperty, null, _script, null);
			}
			catch
			{
				var ex = _engine.ExtractSiteException();
				if (ex != null)
					throw ex;

				throw;
			}
		}

		public void SetValue(string propName, object value)
		{
			if (_script == null)
				throw new InvalidOperationException();

			if (propName == null)
				throw new ArgumentNullException("propName");

			try
			{
				_script.GetType().InvokeMember(propName, BindingFlags.SetProperty, null, _script, new[] { value });
			}
			catch
			{
				var ex = _engine.ExtractSiteException();
				if (ex != null)
					throw ex;

				throw;
			}
		}

		#endregion
	}
}
