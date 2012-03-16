﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using Gel.Scripting;
using Gel.Scripting.JScript;
using Gel.IO;

namespace Gel
{
	public sealed class App
	{
		static App _current;
		public static App Current
		{
			get { return _current; }
			private set
			{
				if (_current != null)
					throw new InvalidOperationException("Application.Current already created.");
				_current = value;
			}
		}

		readonly ProcessApi _processApi;

		App()
		{
			Current = this;

			InitPaths();

			InitWinForms();

			_processApi = new ProcessApi();
		}

		#region Paths

		public string BinPath { get; private set; }

		void InitPaths()
		{
			BinPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
		}

		#endregion

		//bool _exited;

		public void Exit()
		{
			Application.Exit();

			//_exited = true;
		}

		Uri GetCoreScriptUri()
		{
			var scriptFileNames = new[] {
				"Gel.process.js",
				"Gel.evals.js",
				"Gel.core.js",
			};
			var path = System.IO.Path.Combine(BinPath, "core.html");
			//MsieScript.MsieScriptEngine.CreateScriptPage(path, scriptFileNames);
			return new Uri("file://" + path);
		}

		static void InitWinForms()
		{
			/// Standard winforms initialization startup boiler-plate.
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
		}

		[STAThread]
		static void Main()
		{
			var app = new App();

			app.Run();
		}

		public JsEngine Script { get; private set; }

		public void Run()
		{
			Debug.Print("Starting up.");

			/// Create the script engine.
			Script = new JsEngine()
				.SetNamedItem("external", _processApi);

			var corePreprocSrc = JsEngine.PreprocessEmbedded("Gel.gel.js");

			Script.Exec(corePreprocSrc);

			///// TODO: If get main view from script; pass it to Application.Run().
			//if (!_exited)
				Application.Run();

			Debug.Print("Exiting.");
		}
	}
}
