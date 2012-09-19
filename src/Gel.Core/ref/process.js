/// <reference path="external.js"/>

var process = (function(api) {
	/// <param name="api" type="__gel.ProcessApi"/>
	delete external;

	function Process() {
		/// <summary>Process class.</summary>
		/// <field name="argc" type="Number" integer="true">Count of arguments.</field>
		/// <field name="argv" type="Array">Array of arguments.</field>
		/// <field name="moduleLoadList">Just a shim for now.</field>
		this.argc = 0;
		this.argv = [""];
		this.argc = api.argc;
		this.argv = argvFromApi(0, this.argc);
		this.moduleLoadList = [];
	}

	Process.prototype.alert = function(message) {
		/// <summary>Standard alert function.</summary>
	};

	Process.prototype.binding = function(name) {
		/// <summary>Gets the Javascript binding object by name.</summary>
		if (name == 'natives') {
			return {
				"exampleNativeModuleName": "process.alert('example source');"
			};
		}
		return api.binding(type);
	};
	Process.prototype.binding.set = function(name, value) {
		/// <summary>Sets the Javascript value for the given binding name.</summary>
		/// <param name="name">Name of the binding.</param>
		/// <param name="value">Javascript value for the binding.</param>
	}

	Process.prototype.exit = function() {
		/// <summary>Causes the process to exit.</summary>
	};

	Process.prototype.api = function(name) {
		/// <summary>Gets the native API object by name.</summary>
		return api.getApi(name);
	}

	return new Process();
})(external);