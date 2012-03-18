/// <reference path="ref/process.js"/>

process.binding.set('buffer', (function() {
	
	var api = process.api('buffer');
	
	function __isString(value) {
		return typeof obj === 'string' || Object.prototype.toString.call(obj) === '[object String]';
	}

	function ParseEncoding(encoding, defaultEncoding) {
		if (!__isString(encoding))
			return defaultEncoding;
		
		// enum encoding {ASCII, UTF8, BASE64, UCS2, BINARY, HEX};

		if (encoding === "utf8") {
			return 1; // UTF8
		} else if (encoding === "utf-8") {
			return 1; // UTF8
		} else if (encoding === "ascii") {
			return 0; // ASCII
		} else if (encoding === "base64") {
			return 2; // BASE64
		} else if (encoding === "ucs2") {
			return 3; // UCS2
		} else if (encoding === "ucs-2") {
			return 3; // UCS2
		} else if (encoding === "binary") {
			return 4; // BINARY
		} else if (encoding === "hex") {
			return 5; // HEX
		} else if (encoding === "raw") {
//		fprintf(stderr, "'raw' (array of integers) has been removed. "
//					"Use 'binary'.\n");
			return 4; // BINARY
		} else if (encoding === "raws") {
//		fprintf(stderr, "'raws' encoding has been renamed to 'binary'. "
//					"Please update your code.\n");
			return 4; // BINARY
		} else {
			return defaultEncoding;
		}
	}

	function SlowBuffer() {
		
	}

	// ## Static methods

	SlowBuffer.byteLength = function(value, encoding) {
		if (!__isString(value))
			throw Error("Argument must be a string");

		var e = ParseEncoding(encoding, 1); // 1 = UTF8

		return api.byteLength(value, e);
	};

	SlowBuffer.makeFastBuffer = function(buffer, fastBuffer, offset, length) {
		
	};

	// ## Instance methods

	SlowBuffer.prototype.binarySlice = function() { };
	SlowBuffer.prototype.asciiSlice = function() { };
	SlowBuffer.prototype.base64Slice = function() { };
	SlowBuffer.prototype.ucs2Slice = function() { };
	SlowBuffer.prototype.utf8Slice = function() { };

	SlowBuffer.prototype.utf8Write = function() { };
	SlowBuffer.prototype.asciiWrite = function() { };
	SlowBuffer.prototype.binaryWrite = function() { };
	SlowBuffer.prototype.base64Write = function() { };
	SlowBuffer.prototype.ucs2Write = function() { };
	SlowBuffer.prototype.fill = function() { };
	SlowBuffer.prototype.copy = function() { };

	// ## Return binding object.

	return { SlowBuffer: SlowBuffer };
})());