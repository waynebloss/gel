/// <reference path="ref/external.js"/>

var process = (function(api) {
	/// <param name="api" type="__gel.ProcessApi">Host Process API.</param>
	delete external;
	
	function Process() {
		/// <field name="argc" type="Number" integer="true">Count of arguments.</field>
		/// <field name="argv" type="Array">Array of arguments.</field>
		this.argc = api.argc;
		this.argv = argvFromApi(0, this.argc);
	}

	Process.prototype.alert = function(message) {
		/// <summary>Standard alert function.</summary>
		api.alert(message);
	};

	Process.prototype.binding = function(type) {
		return api.binding(type);
	};

	Process.prototype.exit = function() {
		api.exit();
	};

	function argvFromApi(start, count) {
		/// <returns type="Array"></returns>
		var argv = [];
		for (var i = start; i < count; i++) {
			argv[i] = api.argv(i);
		}
		return argv;
	}

	return new Process();

	// The external object that is provided by the host program is basically
	// the api object that the Process class wraps.
})(external);

