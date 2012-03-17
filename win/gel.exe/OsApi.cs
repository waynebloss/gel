using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Gel
{
	[ComVisible(true)]
	public class OsApi
	{
		readonly string _verString;

		internal OsApi()
		{
			var ver = System.Environment.OSVersion.Version;
			_verString = String.Concat(ver.Major.ToString(), ".", ver.Minor.ToString(), ".", ver.Build.ToString());
		}

		public string getHostname()
		{
			return System.Net.Dns.GetHostName();
		}

		public string getLoadAvg()
		{
			// Implemented similarly to Node.js, see file 
			// node/deps/uv/src/win/util.c, function uv_loadavg(double avg[3])
			return "[0, 0, 0]";
		}

		public double getUptime()
		{
			return (double)System.Environment.TickCount / 1000d;
		}

		#region Memory

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		class MEMORYSTATUSEX
		{
			public uint dwLength;
			public uint dwMemoryLoad;
			public ulong ullTotalPhys;
			public ulong ullAvailPhys;
			public ulong ullTotalPageFile;
			public ulong ullAvailPageFile;
			public ulong ullTotalVirtual;
			public ulong ullAvailVirtual;
			public ulong ullAvailExtendedVirtual;
			public MEMORYSTATUSEX()
			{
				this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
			}
		}

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

		public double getFreeMem()
		{
			MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
			if (GlobalMemoryStatusEx(memStatus))
			{
				return (double)memStatus.ullAvailPhys;
			}
			return 0d;
		}

		public double getTotalMem()
		{
			MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
			if (GlobalMemoryStatusEx(memStatus))
			{
				return (double)memStatus.ullTotalPhys;
			}
			return 0d;
		}

		#endregion

		public string getCPUs()
		{
			// TODO: Implement getCPUs similarly to how Node.js does it (via win32 api calls and registry info).
			throw new NotImplementedException();
		}

		public string getOSType()
		{
			// Returns the same thing as Node.js v0.6 on win32.
			return "Windows_NT";
		}

		public string getOSRelease()
		{
			return _verString;
		}

		public string getInterfaceAddresses()
		{
			// Returns the same thing as Node.js v0.6 on win32.
			return "{}";
		}
	}
}
