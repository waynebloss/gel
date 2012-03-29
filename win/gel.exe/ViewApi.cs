using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Gel
{
	/// <summary>
	/// TODO: Figure out if we want to map views based on a key or if we just
	/// want to pass a standard view interface back to the script.
	/// </summary>
	[ComVisible(true)]
	public class ViewApi
	{
		public void alert(string message)
		{
			MessageBox.Show(message, "gel", MessageBoxButtons.OK);
		}

		public bool load(string spec, string id)
		{
			return true;
		}

		public bool hide(int id)
		{
			return true;
		}

		public bool show(int id)
		{
			return true;
		}
	}
}
