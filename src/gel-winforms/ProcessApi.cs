using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using Gel.IO;
using Gel.Scripting;

namespace Gel
{
	public sealed class ProcessApi
	{
		internal ProcessApi()
		{
			_argv = InitArgs(out _evalString, out _printEval);
			_isEval = _evalString != null;
			_api = 
				new [] {
					new { Key = "console", Val = (object)new ConsoleApi() },
					new { Key = "evals", Val = (object)new EvalsApi() },
					new { Key = "natives", Val = (object)new NativesApi() },
					new { Key = "process", Val = (object)this },
					new { Key = "timers", Val = (object)new TimersApi() },
					new { Key = "os", Val = (object)new OsApi() },
					new { Key = "fs", Val = (object)new FsApi() }
				}
				.ToDictionary(k => k.Key, v => v.Val);
		}

		#region Arguments

		/// <summary>
		/// The list of arguments that were passed to the process.
		/// </summary>
		readonly string[] _argv;
		/// <summary>
		/// Gets the count of arguments that were passed to the process.
		/// </summary>
		public int argc { get { return _argv.Length; } }
		/// <summary>
		/// Gets an item from the list of arguments that were passed to the process.
		/// <para>(argv stands for 'argument vector'.)</para>
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public string argv(int index) { return _argv[index]; }
		/// <summary>
		/// Gets the process arguments array ready for the scripting environment.
		/// <para>Details in remarks.</para>
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// The first element of the array returned by <see cref="Environment.GetCommandLineArgs"/>
		/// is always the path that was used to execute the main executable.
		/// For instance, if you run "C:\bin\myapp.exe", the first argument will be that string.
		/// If "C:\bin\" is in the global path though, when you run "myapp.exe", just "myapp.exe"
		/// will be the first argument.
		/// 
		/// Instead of making our scripts figure this out, we just replace the first argument with
		/// the full path to the entry assembly right here.
		/// </remarks>
		static string[] InitArgs(out string evalStr, out bool printEval)
		{
			evalStr = null;
			printEval = false;

			var argv = Environment.GetCommandLineArgs();
			argv[0] = System.Reflection.Assembly.GetEntryAssembly().Location;
			for (int i = 0; i < argv.Length; i++)
			{
				var arg = argv[i];
				if (arg == "--eval" || arg == "-e" || arg == "-pe")
				{
					if (argv.Length <= i + 1)
						throw new ArgumentException("--eval, -e or -pe requires an argument.");
					if (arg[1] == 'p')
						printEval = true;

					for (int j = i + 1; j < argv.Length; j++)
						evalStr = evalStr + argv[j] + " ";

					argv = argv.Take(i + 1).ToArray();

					Debug.Print("Evaling: " + evalStr);

					break;
				}
			}
			return argv;
		}

		#endregion

		Dictionary<string, object> _api;

		public void alert(string message)
		{
			MessageBox.Show(message, "@alertTitle", MessageBoxButtons.OK);
		}

		public void exit()
		{
			Debug.Print("process.exit();");
			App.Current.Exit();
		}

		public object getApi(string name)
		{
			object value;
			_api.TryGetValue(name, out value);
			return value;
		}

		public void addTestExpression()
		{

		}

		public void addTestScript()
		{
			App.Current.Script.Exec(ScriptEmbedded.ReadFile("Gel.Eval.js"));
		}
		
		public void needTickCallback()
		{
			if (!Application.MessageLoop || Application.OpenForms.Count == 0)
			{
				App.Current.needTickCallback = true;
			}
			else
			{
				System.Threading.ThreadPool.QueueUserWorkItem(o =>
				{
					var context = (System.Threading.SynchronizationContext)o;
					context.Post(doTickCallback, null);
				}, System.Threading.SynchronizationContext.Current);
			}
		}

		Type _tickCallbackType;
		object _tickCallback;
		public object tickCallback
		{
			get { return _tickCallback; }
			set
			{
				_tickCallback = value;
				if (value != null)
					_tickCallbackType = value.GetType();
				else
					_tickCallbackType = null;
			}
		}

		internal void doTickCallback(object state)
		{
			App.Current.needTickCallback = false;

			Debug.Print("doTickCallback();");

			App.Current.Script.Exec("process.tickCallback();");
		}

		public string cwd()
		{
			return System.IO.Directory.GetCurrentDirectory();
		}

		readonly bool _printEval;
		public bool printEval { get { return _printEval; } }

		readonly bool _isEval;
		public bool isEval { get { return _isEval; } }

		readonly string _evalString;
		public string evalString { get { return _evalString; } }
	}
}
