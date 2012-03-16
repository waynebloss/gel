// DO NOT REDISTRIBUTE WITHOUT COPYRIGHT LICENSE FILE: ../lic/node.txt

// Query String Utilities

var QueryString = exports;
// unused var: var urlDecode = process.binding('http_parser').urlDecode;


// If obj.hasOwnProperty has been overridden, then calling
// obj.hasOwnProperty(prop) will break.
// See: https://github.com/joyent/node/issues/1707
function hasOwnProperty(obj, prop) {
  return Object.prototype.hasOwnProperty.call(obj, prop);
}


function charCode(c) {
  return c.charCodeAt(0);
}


// a safe fast alternative to decodeURIComponent
QueryString.unescapeBuffer = function(s, decodeSpaces) {
	throw Error("QueryString.unescapeBuffer is not supported.");
//  var out = new Buffer(s.length);
//  var state = 'CHAR'; // states: CHAR, HEX0, HEX1
//  var n, m, hexchar;

//  for (var inIndex = 0, outIndex = 0; inIndex <= s.length; inIndex++) {
//    var c = s.charCodeAt(inIndex);
//    switch (state) {
//      case 'CHAR':
//        switch (c) {
//          case charCode('%'):
//            n = 0;
//            m = 0;
//            state = 'HEX0';
//            break;
//          case charCode('+'):
//            if (decodeSpaces) c = charCode(' ');
//            // pass thru
//          default:
//            out[outIndex++] = c;
//            break;
//        }
//        break;

//      case 'HEX0':
//        state = 'HEX1';
//        hexchar = c;
//        if (charCode('0') <= c && c <= charCode('9')) {
//          n = c - charCode('0');
//        } else if (charCode('a') <= c && c <= charCode('f')) {
//          n = c - charCode('a') + 10;
//        } else if (charCode('A') <= c && c <= charCode('F')) {
//          n = c - charCode('A') + 10;
//        } else {
//          out[outIndex++] = charCode('%');
//          out[outIndex++] = c;
//          state = 'CHAR';
//          break;
//        }
//        break;

//      case 'HEX1':
//        state = 'CHAR';
//        if (charCode('0') <= c && c <= charCode('9')) {
//          m = c - charCode('0');
//        } else if (charCode('a') <= c && c <= charCode('f')) {
//          m = c - charCode('a') + 10;
//        } else if (charCode('A') <= c && c <= charCode('F')) {
//          m = c - charCode('A') + 10;
//        } else {
//          out[outIndex++] = charCode('%');
//          out[outIndex++] = hexchar;
//          out[outIndex++] = c;
//          break;
//        }
//        out[outIndex++] = 16 * n + m;
//        break;
//    }
//  }

//  // TODO support returning arbitrary buffers.

//  return out.slice(0, outIndex - 1);
};


QueryString.unescape = function(s, decodeSpaces) {
//	var uri="http://w3schools.com/my test.asp?name=ståle&car=saab";
//	var uri_encode=encodeURIComponent(uri);
//	console.log(uri_encode);
//	console.log("<br />");
//	console.log(decodeURIComponent(uri_encode));
  
  //return QueryString.unescapeBuffer(s, decodeSpaces).toString();

  if (decodeSpaces !== undefined)
	console.warn("QueryString.unescape decodeSpaces argument not implemented.");

	return decodeURIComponent(s);
};


QueryString.escape = function(str) {
  return encodeURIComponent(str);
};

var stringifyPrimitive = function(v) {
  switch (typeof v) {
    case 'string':
      return v;

    case 'boolean':
      return v ? 'true' : 'false';

    case 'number':
      return isFinite(v) ? v : '';

    default:
      return '';
  }
};


QueryString.stringify = QueryString.encode = function(obj, sep, eq, name) {
  sep = sep || '&';
  eq = eq || '=';
  obj = (obj === null) ? undefined : obj;

  switch (typeof obj) {
    case 'object':
      return Object.keys(obj).map(function(k) {
        if (Array.isArray(obj[k])) {
          return obj[k].map(function(v) {
            return QueryString.escape(stringifyPrimitive(k)) +
                   eq +
                   QueryString.escape(stringifyPrimitive(v));
          }).join(sep);
        } else {
          return QueryString.escape(stringifyPrimitive(k)) +
                 eq +
                 QueryString.escape(stringifyPrimitive(obj[k]));
        }
      }).join(sep);

    default:
      if (!name) return '';
      return QueryString.escape(stringifyPrimitive(name)) + eq +
             QueryString.escape(stringifyPrimitive(obj));
  }
};

// Parse a key=val string.
QueryString.parse = QueryString.decode = function(qs, sep, eq, options) {
  sep = sep || '&';
  eq = eq || '=';
  var obj = {},
      maxKeys = 1000;

  // Handle maxKeys = 0 case
  if (options && typeof options.maxKeys === 'number') {
    maxKeys = options.maxKeys;
  }

  if (typeof qs !== 'string' || qs.length === 0) {
    return obj;
  }

  var regexp = /\+/g;
  qs = qs.split(sep);

  // maxKeys <= 0 means that we should not limit keys count
  if (maxKeys > 0) {
    qs = qs.slice(0, maxKeys);
  }

  for (var i = 0, len = qs.length; i < len; ++i) {
    var x = qs[i].replace(regexp, '%20'),
        idx = x.indexOf(eq),
        kstr = x.substring(0, idx),
        vstr = x.substring(idx + 1), k, v;

    try {
      k = decodeURIComponent(kstr);
      v = decodeURIComponent(vstr);
    } catch (e) {
      k = QueryString.unescape(kstr, true);
      v = QueryString.unescape(vstr, true);
    }

    if (!hasOwnProperty(obj, k)) {
      obj[k] = v;
    } else if (!Array.isArray(obj[k])) {
      obj[k] = [obj[k], v];
    } else {
      obj[k].push(v);
    }
  }

  return obj;
};
