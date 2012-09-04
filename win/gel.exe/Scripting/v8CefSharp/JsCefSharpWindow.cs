using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CefSharp;

namespace Gel.Scripting.v8CefSharp
{
	[System.ComponentModel.DesignerCategory("")] 
	class JsCefSharpWindow : Form
	{
		static JsCefSharpWindow()
		{
			var globalCefSettings = new CefSharp.Settings();

			if (CEF.Initialize(globalCefSettings))
			{
				CEF.RegisterScheme("gel", new GelSchemeHandlerFactory());
			}
		}

		public JsCefSharpWindow(string address, BrowserSettings settings)
		{
			_WebView = new CefSharp.WinForms.WebView(address, settings);
			
			InitializeComponent();
		}

		readonly CefSharp.WinForms.WebView _WebView;

		public CefSharp.WinForms.WebView WebView { get { return _WebView;  } }

		void InitializeComponent()
		{
			this.SuspendLayout();
			//
			// _web
			//
			_WebView.Dock = DockStyle.Fill;
			// 
			// CefWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 600);
			this.Controls.Add(_WebView);
			this.Name = "CefWindow";
			this.Text = "CefWindow";
			this.ResumeLayout(false);
		}
	}
}
