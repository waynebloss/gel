/// <reference path="external.js"/>

var process = (function(api) {
	/// <param name="api" type="__gel.ProcessApi"/>
	delete external;

	function Process() {
		/// <summary>Process class.</summary>
		/// <field name="argc" type="Number" integer="true">Count of arguments.</field>
		/// <field name="argv" type="Array">Array of arguments.</field>
		this.argc = 0;
		this.argv = [""];
	}

	Process.prototype.alert = function(message) {
		/// <summary>Standard alert function.</summary>
	};

	Process.prototype.binding = function(type) {
		/// <summary>Returns a native module binding object.</summary>
		return api.binding(type);
	};

	Process.prototype.exit = function() {
		/// <summary>Causes the process to exit.</summary>
	};

	return new Process();
})(external);