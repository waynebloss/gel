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

		for (var i in global) {
			console.log('global.' + i + ': ' + typeof global[i] + 
				(global[i] === null ? ' (null)' : ''));
		}

		var Script = process.binding('evals').NodeScript;
		var x = Script.runInThisContext('a = 1;', '', true);
		console.log('VALUE OF x: ' + x);
		console.log('typeof a: ' + typeof a);
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
