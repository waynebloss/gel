using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gel
{
	public static class StringUtil
	{
		public static bool IsHex(this string s)
		{
			for (int i = 0; i < s.Length; i++)
				if (!s[i].IsHex())
					return false;

			return true;
		}

		public static bool IsGuid(this string s)
		{
			// Length of a proper GUID, without any surrounding braces.
			const int len_without_braces = 36;

			// Delimiter for GUID data parts.
			const char delim = '-';

			// Delimiter positions.
			const int d_0 = 8;
			const int d_1 = 13;
			const int d_2 = 18;
			const int d_3 = 23;

			// Before Delimiter positions.
			const int bd_0 = 7;
			const int bd_1 = 12;
			const int bd_2 = 17;
			const int bd_3 = 22;

			if (s == null)
				return false;

			if (s.Length != len_without_braces)
				return false;

			if (s[d_0] != delim ||
				s[d_1] != delim ||
				s[d_2] != delim ||
				s[d_3] != delim)
				return false;

			for (int i = 0;
				i < s.Length;
				i = i + (i == bd_0 ||
						i == bd_1 ||
						i == bd_2 ||
						i == bd_3
						? 2 : 1))
			{
				if (!s[i].IsHex()) return false;
			}

			return true;
		}
	}
}
