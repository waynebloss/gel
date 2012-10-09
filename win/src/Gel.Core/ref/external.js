var __gel = {};

var external = (function() {

	var console = {};

	console.log = function(output) {
		/// <summary>Prints to the console.</summary>
		/// <param name="output">The string to log.</param>
	};
	console.info = console.log;
	console.warn = function(output) {
		/// <summary>Same as console.log but prints to stderr.</summary>
	};
	console.error = console.warn;
	console.dir = function(object) {
		/// <summary>Uses util.inspect on object and prints resulting string to stderr.</summary>
		/// <param name="object">The object to inspect.</param>
	};
	console.time = function(label) {
		/// <summary>Mark a time.</summary>
		/// <param name="label">Label for the time.</param>
	};
	console.timeEnd = function(label) {
		/// <summary>Finish timer started by console.time, record output.</summary>
		/// <param name="label">Label for the time.</param>
	};
	console.trace = function(label) {
		/// <summary>Print a stack trace to stderr of the current position.</summary>
		/// <param name="label">Label for the stack trace.</param>
	};
	console.assert = function(expression) {
		/// <summary>Same as assert.ok().</summary>
		/// <param name="expression">The expression to assert.</param>
	};

	function ProcessApi() {
		/// <summary>External Api object.</summary>
		/// <field name="argc" type="Number" integer="true">Count of arguments.</field>
		this.argc = 2;
	}
	ProcessApi.prototype.argv = function(index) {
		/// <summary>Gets the argument from the given index.</summary>
		return "";
	};
	ProcessApi.prototype.getApi = function(name) {
		/// <summary>Returns the native API object.</summary>
		/// <param name="name" type="String">Name of the API to return.</param>
		switch (name) {
			case 'console':
				return console;
			case 'natives':
				return {
					util: "// util source."
				};
			default:
				return {};
		}
	};
	ProcessApi.prototype.exit = function() {
		/// <summary>Exits the process.</summary>
	};
	__gel.ProcessApi = ProcessApi;

	return new ProcessApi();
})();