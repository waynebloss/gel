using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;

namespace Gel.Scripting.ActiveScript
{

	static class ComConstants
	{
		public const int INTERFACE_USES_SECURITY_MANAGER = 0x00000008; // Object knows to use IInternetHostSecurityManager
	}

	enum HRESULT
	{
		TYPE_E_ELEMENTNOTFOUND = unchecked((int)0x8002802B),
		SCRIPT_E_REPORTED = unchecked((int)0x80020101),
		E_NOTIMPL = unchecked((int)0x80004001),
		E_NOINTERFACE = unchecked((int)0x80004002),
		S_OK = 0x00000000,
		S_FALSE = 0x00000001
	}

	[Flags]
	enum ScriptInfo : uint
	{
		None = 0x0000,
		IUnknown = 0x0001,
		ITypeInfo = 0x0002,
	}

	[Flags]
	enum ScriptItem : uint
	{
		None = 0x0000,
		IsVisible = 0x0002,
		IsSource = 0x0004,
		GlobalMembers = 0x0008,
		IsPersistent = 0x0040,
		CodeOnly = 0x0200,
		NoCode = 0x0400,
	}

	[Flags]
	enum ScriptText : uint
	{
		None = 0x0000,

		DelayExecution = 0x0001,
		IsVisible = 0x0002,
		IsExpression = 0x0020,
		IsPersistent = 0x0040,
		HostManageSource = 0x0080,
	}

	enum ScriptThreadState : uint
	{
		NotInScript = 0,
		Running = 1,
	}

	enum ScriptState : uint
	{
		Uninitialized = 0,
		Started = 1,
		Connected = 2,
		Disconnected = 3,
		Closed = 4,
		Initialized = 5
	}

	enum UrlPolicy
	{
		DisAllow = 0x03
	}
}
