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
	[ComVisible(true)]
	public sealed class ProcessApi
	{
		internal ProcessApi()
		{
			_argv = InitArgs();
			_api = 
				new [] {
					new { Key = "console", Val = (object)new ConsoleApi() },
					new { Key = "evals", Val = (object)new EvalsApi() },
					new { Key = "natives", Val = (object)new NativesApi() },
					new { Key = "process", Val = (object)this },
					new { Key = "timers", Val = (object)new TimersApi() },
					new { Key = "os", Val = (object)new OsApi() },
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
		static string[] InitArgs()
		{
			var argv = Environment.GetCommandLineArgs();
			argv[0] = System.Reflection.Assembly.GetEntryAssembly().Location;
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
			App.Current.Script.Parse(ScriptEmbedded.ReadFile("Gel.Eval.js"));
		}
	}
}
