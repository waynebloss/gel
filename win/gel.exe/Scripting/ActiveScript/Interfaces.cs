using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using EXCEPINFO = System.Runtime.InteropServices.ComTypes.EXCEPINFO;
using ITypeInfo = System.Runtime.InteropServices.ComTypes.ITypeInfo;
using DISPPARAMS = System.Runtime.InteropServices.ComTypes.DISPPARAMS;

namespace Gel.Scripting.ActiveScript
{
	[ComImport]
	[Guid("00020400-0000-0000-c000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IDispatch
	{
		int GetTypeInfoCount();
		ITypeInfo GetTypeInfo(
			[MarshalAs(UnmanagedType.U4)] int iTInfo,
			[MarshalAs(UnmanagedType.U4)] int lcid);

		[PreserveSig]
		int GetIDsOfNames(ref Guid riid,
						  [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] rgsNames,
						  int cNames,
						  int lcid,
						  [MarshalAs(UnmanagedType.LPArray)] int[] rgDispId);

		[PreserveSig]
		int Invoke(int dispIdMember,
				   ref Guid riid,
				   [MarshalAs(UnmanagedType.U4)] int lcid,
				   [MarshalAs(UnmanagedType.U4)] int dwFlags,
				   ref DISPPARAMS pDispParams,
				   [Out, MarshalAs(UnmanagedType.LPArray)] object[] pVarResult,
				   ref EXCEPINFO pExcepInfo,
				   [Out, MarshalAs(UnmanagedType.LPArray)] IntPtr[] pArgErr);
	}
	/// <summary>
	/// A stub interface to use to interact with the script itself through IDispatch.<para>
	/// It uses the IDispatch guid.  New methods can be added as needed in any order.</para>
	/// </summary>
	[ComImport]
	[Guid("00020400-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	interface IScriptDispatch
	{
		object FindProxyForURL(string url, string host);
	}

	[ComImport]
	[Guid("BB1A2AE1-A4F9-11cf-8F20-00805F2CD064")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IActiveScript
	{
		void SetScriptSite(IActiveScriptSite pass);
		void GetScriptSite(Guid riid, out IntPtr site);
		void SetScriptState(ScriptState state);
		void GetScriptState(out ScriptState scriptState);
		void Close();
		void AddNamedItem(string name, ScriptItem flags);
		void AddTypeLib(Guid typeLib, uint major, uint minor, uint flags);
		void GetScriptDispatch(string itemName, out IntPtr dispatch);
		void GetCurrentScriptThreadID(out uint thread);
		void GetScriptThreadID(uint win32ThreadId, out uint thread);
		void GetScriptThreadState(uint thread, out ScriptThreadState state);
		void InterruptScriptThread(uint thread, out EXCEPINFO exceptionInfo, uint flags);
		void Clone(out IActiveScript script);
	}

	[ComImport]
	[Guid("DB01A1E3-A42B-11cf-8F20-00805F2CD064")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IActiveScriptSite
	{
		void GetLCID(out int lcid);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="returnMask"></param>
		/// <param name="item"></param>
		/// <param name="typeInfo"></param>
		/// <remarks>
		/// Alternate declaration: void GetItemInfo(string name, ScriptInfo returnMask, out IntPtr item, IntPtr typeInfo);
		/// </remarks>
		void GetItemInfo(
			string name,
			ScriptInfo returnMask,
			[Out] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.IUnknown)] object[] item,
			[Out] [MarshalAs(UnmanagedType.LPArray)] IntPtr[] typeInfo);

		void GetDocVersionString(out string version);
		void OnScriptTerminate(object result, EXCEPINFO exceptionInfo);
		void OnStateChange(ScriptState scriptState);
		void OnScriptError(IActiveScriptError scriptError);
		void OnEnterScript();
		void OnLeaveScript();
	}

	[ComImport()]
	[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
	[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	interface IOleServiceProvider
	{
		[PreserveSig]
		int QueryService([In] ref Guid guidService, [In] ref Guid riid, [Out] out IntPtr ppvObject);
	}

	[ComImport]
	[Guid("CB5BDC81-93C1-11cf-8F20-00805F2CD064")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IObjectSafety
	{
		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetInterfaceSafetyOptions(ref Guid riid, [Out] out int pdwSupportedOptions, [Out] out int pdwEnabledOptions);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions);
	}

	[ComImport]
	[Guid("3af280b6-cb3f-11d0-891e-00c04fb6bfc4")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IInternetHostSecurityManager
	{
		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int GetSecurityId([Out] byte[] pbSecurityId, [In, Out]ref IntPtr pcbSecurityId, IntPtr dwReserved);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int ProcessUrlAction(int dwAction, [Out] int[] pPolicy, int cbPolicy, [Out] byte[] pContext, int cbContext, int dwFlags, int dwReserved);

		[return: MarshalAs(UnmanagedType.I4)]
		[PreserveSig]
		int QueryCustomPolicy(Guid guidKey, [Out] out byte[] ppPolicy, [Out] out int pcbPolicy, byte[] pContext, int cbContext, int dwReserved);
	}

	[ComImport]
	[Guid("EAE1BA61-A4ED-11cf-8F20-00805F2CD064")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IActiveScriptError
	{
		void GetExceptionInfo(out EXCEPINFO exceptionInfo);
		void GetSourcePosition(out uint sourceContext, out uint lineNumber, out int characterPosition);
		void GetSourceLineText(out string sourceLine);
	}

	[ComImport]
	[Guid("BB1A2AE2-A4F9-11cf-8F20-00805F2CD064")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IActiveScriptParse32
	{
		void InitNew();
		
		void AddScriptlet(
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
			out EXCEPINFO exceptionInfo);

		void ParseScriptText(
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
			out EXCEPINFO exceptionInfo);
	}

	[ComImport]
	[Guid("C7EF7658-E1EE-480E-97EA-D52CB4D76D17")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IActiveScriptParse64
	{
		void InitNew();

		void AddScriptlet(
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
			out EXCEPINFO exceptionInfo);

		void ParseScriptText(
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
			out EXCEPINFO exceptionInfo);
	}

	[ComImport]
	[Guid("4954E0D0-FBC7-11D1-8410-006008C3FBFC")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	interface IActiveScriptProperty
	{
		void GetProperty(ScriptProp dwProperty, IntPtr pvarIndex, [Out] out object pvarValue);
		void SetProperty(ScriptProp dwProperty, IntPtr pvarIndex, [In] ref object pvarValue);
	}
}
