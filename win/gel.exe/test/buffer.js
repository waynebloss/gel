/// <reference path="ref/process.js"/>

exports.exec = function() {
		// Ideas for Buffer class:
		var arr = [0, 1, 2];
		arr[3] = 3;
		console.log('arr has ' + arr.length + ' items.');

		function ArrayBuffer(length) {

			// The basica idea here is to pre-fill this array
			// and tack our custom methods onto this instance
			// to avoid modifying Array's prototype.
			this.init(0, length);

			this.custFn = function() { 
				return 'ArrayBuffer has ' + this.length + ' items.';
			};
		}
		ArrayBuffer.prototype = Array.prototype;

		testBuffer(ArrayBuffer, 3);

		function Buffer(length) {
			// Prefilling this "array" here.
			this.length = length;
			while (length--) { this[length] = 0; }
			//length = this.length;
		}
		Buffer.prototype.custFn = function() {
			return 'Buffer has ' + this.length + ' items.';
		};

		testBuffer(Buffer, 3);
};

function testBuffer(BufferClass, len) {
	var x = new BufferClass(len);
	console.log(x.custFn());
	for (i = 0; i < x.length; i++) console.log(x[i]);
}