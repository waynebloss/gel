using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gel
{
	public static class CharUtil
	{
		public static bool IsHex(this char c)
		{
			return ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'));
		}
	}
}
