using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Gel.Scripting.NodeJs
{
	public class NodeProcess
	{
		Process _proc;
		bool _running;
		bool _initialized;
		int _exitCode;

		public event EventHandler Exited;

		public void Kill()
		{
			if (_proc != null && !_proc.HasExited)
				_proc.Kill();
		}

		public void Run(string script)
		{
			var si = new ProcessStartInfo("node.exe", script);

			si.CreateNoWindow = true;
			si.ErrorDialog = false;
			si.UseShellExecute = false;

			si.RedirectStandardError = true;
			si.RedirectStandardInput = true;
			si.RedirectStandardOutput = true;

			try
			{
				_proc = new Process();
				_proc.StartInfo = si;

				_proc.OutputDataReceived += new DataReceivedEventHandler(proc_OutputDataReceived);
				_proc.ErrorDataReceived += new DataReceivedEventHandler(proc_ErrorDataReceived);
				_proc.Exited += new EventHandler(proc_Exited);

				_running = _proc.Start();

				if (!_running)
					throw new ApplicationException("Couldn't start process node.exe");

				_proc.BeginErrorReadLine();
				_proc.BeginOutputReadLine();
			}
			catch
			{
				if (_proc != null)
				{
					_exitCode = _proc.ExitCode;
					_proc.Dispose();
					_proc = null;
				}
				throw;
			}
		}

		void proc_Exited(object sender, EventArgs e)
		{
			Debug.Print("proc Exited.");

			_exitCode = _proc.ExitCode;

			_proc.Dispose();
			_proc = null;

			var handler = this.Exited;
			if (handler != null)
				handler(this, EventArgs.Empty);
		}

		void proc_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			Debug.Print("ERROR: " + e.Data);
		}

		void proc_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			Debug.Print(e.Data);
			if (!_initialized)
			{
				_initialized = true;

				_proc.StandardInput.WriteLine(@"{""event"": {""id"": ""data""}}");

				return;
			}
			//if (e.Data == "connecting to: local")
			//{
			//    _proc.StandardInput.WriteLine("exit");
			//    _proc.StandardInput.WriteLine("exit");
			//    return;
			//}
			//_proc.WaitForExit(0);
		}
	}
}
