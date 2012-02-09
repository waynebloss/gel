using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gel
{
	/// <summary>
	/// Basic array data structure used by the Gel framework.
	/// </summary>
	public class GelArray : IEnumerable<object>
	{
		/// <summary>
		/// The list of [<see cref="GelArray"/>, a <see cref="GelObject"/>, a <see cref="String"/> or null] values.
		/// </summary>
		readonly List<object> _v = new List<object>();

		public GelArray Add(object value)
		{
			_v.Add(value);
			return this;
		}

		#region IEnumerable<object>

		public IEnumerator<object> GetEnumerator()
		{
			return _v.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _v.GetEnumerator();
		}

		#endregion
	}
}
