using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Gel
{
	public enum EncodingEnum { ASCII, UTF8, BASE64, UCS2, BINARY, HEX }

	[ComVisible(true)]
	public class BufferApi
	{
		internal BufferApi() { }
		System.Text.Encoding thing = null;

		static int base64_decoded_size(string src, int size)
		{
			int remainder = size % 4;

			size = (size / 4) * 3;
			if (remainder > 0)
			{
				if (size == 0 && remainder == 1)
				{
					// special case: 1-byte input cannot be decoded
					size = 0;
				}
				else
				{
					// non-padded input, add 1 or 2 extra bytes
					size += 1 + (remainder == 3 ? 1 : 0);
				}
			}
			// check for trailing padding (1 or 2 bytes)
			if (size > 0)
			{
				if (src[size - 1] == '=') size--;
				if (src[size - 2] == '=') size--;
			}
			return size;
		}

		public int byteLength(string value, int enc)
		{
			var encoding = (EncodingEnum)enc;
			switch (encoding)
			{
			case EncodingEnum.UTF8:
				return System.Text.UTF8Encoding.UTF8.GetByteCount(value);
			case EncodingEnum.BASE64:
				return base64_decoded_size(value, value.Length);
			case EncodingEnum.UCS2:
				return value.Length * 2;
			case EncodingEnum.HEX:
				return value.Length / 2;
			default:
				return value.Length;
			}
		}
	}
}
