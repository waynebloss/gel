using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gel
{
	/// <summary>
	/// Basic object data structure used by the Gel framework.
	/// </summary>
	public class GelObject : IEnumerable<object>
	{
		/// <summary>
		/// The dictionary of [<see cref="GelArray"/>, a <see cref="GelObject"/>, a <see cref="String"/> or null] values.
		/// </summary>
		readonly Dictionary<string, object> _v = new Dictionary<string, object>();

		public GelObject Add(string key, object value)
		{
			_v.Add(key, value);
			return this;
		}

		public ICollection<string> Keys { get { return _v.Keys; } }

		public ICollection<object> Values { get { return _v.Values; } }

		public object this[string key]
		{
			get { return _v[key]; }
			set { _v[key] = value; }
		}

		#region Get

		public T Get<T>(string key, T defaultValue)
			where T : class
		{
			object val;
			if (!_v.TryGetValue(key, out val))
			{
				return defaultValue;
			}
			var tval = val as T;
			if (tval != null)
				return defaultValue;
			else
				return tval;
		}

		public GelObject GetGraph(string key)
		{
			return Get<GelObject>(key, null);
		}

		public GelObject GetGraph(string key, GelObject defaultValue)
		{
			return Get<GelObject>(key, defaultValue);
		}

		public GelArray GetList(string key)
		{
			return Get<GelArray>(key, null);
		}

		public GelArray GetList(string key, GelArray defaultValue)
		{
			return Get<GelArray>(key, defaultValue);
		}

		public string GetString(string key)
		{
			return Get<string>(key, null);
		}

		public string GetString(string key, string defaultValue)
		{
			return Get<string>(key, defaultValue);
		}

		#endregion

		#region TryGet

		public bool TryGet<T>(string key, out T value)
			where T : class
		{
			object val;
			if (!_v.TryGetValue(key, out val))
			{
				value = null;
				return false;
			}
			value = val as T;
			return value != null;
		}

		public bool TryGetString(string key, out string value)
		{
			return TryGet<string>(key, out value);
		}

		public bool TryGetList(string key, out GelArray value)
		{
			return TryGet<GelArray>(key, out value);
		}

		public bool TryGetGraph(string key, out GelObject value)
		{
			return TryGet<GelObject>(key, out value);
		}

		public bool TryGetValue(string key, out object value)
		{
			return _v.TryGetValue(key, out value);
		}

		#endregion

		#region IEnumerable<object>

		public IEnumerator<object> GetEnumerator()
		{
			return _v.Values.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return _v.Values.GetEnumerator();
		}

		#endregion
	}
}
