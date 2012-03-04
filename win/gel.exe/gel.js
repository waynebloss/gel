/// <reference path="ref/process.js"/>
/// <reference path="console.js"/>

// #include <Gel.process.js>
// #include <Gel.natives.js>
// #include <Gel.console.js>
// #include <Gel.evals.js>

var global;

(function() {
	
	global = this;
	
	function startup() {
		startup.globalVariables();
		startup.globalTimeouts();
		
		var util = NativeModule.require('util');
		util.puts("Heloooooooooooooooooooooooooooooooooooo");

	}
	
	startup.globalVariables = function() {
		global.process = process;
		global.global = global;
		global.GLOBAL = global;
		global.root = global;
// TODO: Restore when possible:
//		global.Buffer = NativeModule.require('buffer').Buffer;
	};

	startup.globalTimeouts = function() {

		global.setTimeout = function() {
// TODO: Restore when possible:
//			var t = NativeModule.require('timers');
//			return t.setTimeout.apply(this, arguments);
		};
		global.setInterval = function() {
// TODO: Restore when possible:
//			var t = NativeModule.require('timers');
//			return t.setInterval.apply(this, arguments);
		};
		global.clearTimeout = function() {
// TODO: Restore when possible:
//			var t = NativeModule.require('timers');
//			return t.clearTimeout.apply(this, arguments);
		};
		global.clearInterval = function() {
// TODO: Restore when possible:
//			var t = NativeModule.require('timers');
//			return t.clearInterval.apply(this, arguments);
		};

	};

// #include <Gel.native_module.js>

	startup();
	
})();
