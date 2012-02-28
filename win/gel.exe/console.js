/// <reference path="ref/process.js"/>

var console = (function() {

	var api = process.binding('console');
	var exports = {};

	exports.log = function(message, args) {
		/// <summary>Prints to stdout with newline. This function can take multiple arguments in a printf()-like way.</summary>
		/// <param name="message">The string to print, may include formatting.</param>
		/// <param name="args" optional="true">(Optional) Params applied to given message string.</param>

		// TODO: Format the message using the given arguments, if any.
		api.log(message);
	};
	exports.info = exports.log;
	exports.warn = function() {
		/// <summary>Same as console.log but prints to stderr.</summary>
		throw new Error("Not implemented.");
	};
	exports.error = exports.warn;
	exports.dir = function(object) {
		/// <summary>Uses util.inspect on object and prints resulting string to stderr.</summary>
		/// <param name="object">The object to inspect.</param>
		throw new Error("Not implemented.");
	};
	exports.time = function(label) {
		/// <summary>Mark a time.</summary>
		/// <param name="label">Label for the time.</param>
		throw new Error("Not implemented.");
	};
	exports.timeEnd = function(label) {
		/// <summary>Finish timer started by console.time, record output.</summary>
		/// <param name="label">Label for the time.</param>
		throw new Error("Not implemented.");
	};
	exports.trace = function(label) {
		/// <summary>Print a stack trace to stderr of the current position.</summary>
		/// <param name="label">Label for the stack trace.</param>
		throw new Error("Not implemented.");
	};
	exports.assert = function(expression) {
		/// <summary>Same as assert.ok().</summary>
		/// <param name="expression">The expression to assert.</param>
		throw new Error("Not implemented.");
	};

	return exports;
})();