/// <reference path="ref/process.js"/>
/// <reference path="console.js"/>

// #include <Gel.ddr-ecma5.js>
// #include <Gel.json2.js>
// #include <Gel.process.js>
// #include <Gel.os.js>
// #include <Gel.natives.js>
// #include <Gel.console.js>
// #include <Gel.evals.js>
// #include <Gel.buffer.js>
// #include <Gel.timer_wrap.js>

var global;

(function() {

	global = this;

	function startup() {
		var EventEmitter = NativeModule.require('events').EventEmitter;
		Object.set__proto__(process, EventEmitter.prototype);
		process.EventEmitter = EventEmitter; // process.EventEmitter is deprecated

		startup.globalVariables();
		startup.globalTimeouts();
		startup.globalConsole();

		startup.processAssert();

		// temp stubs for stdio until startup.processStdio is implemented.
		process.stdout = {write:console.log};
		process.stderr = {write:console.log};

//		startup.processStdio();
//		startup.processKillAndExit();
//		startup.processSignalHandlers();

//		startup.processChannel();

//		startup.resolveArgv0();

		startup.printEngineVer();

		process.on('exit', function() {
			console.log('exiting!!!!!!!!!!!!!!!!!!');
		});

		//NativeModule.require('test.assert');
		var test = NativeModule.require('test.index')
		test.exec('os');
		test.exec('path');
		test.exec('buffer');
		test.exec('timers');
		test.exec('process');
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

		global.setTimeout = function(callback, after) {
			var t = NativeModule.require('timers');
			return t.setTimeout.apply(this, arguments);
		};
		global.setInterval = function(callback, repeat) {
			var t = NativeModule.require('timers');
			return t.setInterval.apply(this, arguments);
		};
		global.clearTimeout = function(timer) {
			var t = NativeModule.require('timers');
			return t.clearTimeout.apply(this, arguments);
		};
		global.clearInterval = function(timer) {
			var t = NativeModule.require('timers');
			return t.clearInterval.apply(this, arguments);
		};

	};
	startup.globalConsole = function() {
		// The following commented-out code is not necessary:
		//global.__defineGetter__('console', function() {
		//  return NativeModule.require('console');
		//});
	};

	startup._lazyConstants = null;
	startup.lazyConstants = function() {
		if (!startup._lazyConstants) {
			startup._lazyConstants = process.binding('constants');
		}
		return startup._lazyConstants;
	};

	var assert;
	startup.processAssert = function() {
		// Note that calls to assert() are pre-processed out by JS2C for the
		// normal build of node. They persist only in the node_g build.
		// Similarly for debug().
		assert = process.assert = function(x, msg) {
			if (!x) throw new Error(msg || 'assertion error');
		};
	};

	startup.printEngineVer = function() {
		var ver = ScriptEngine() + '/' + 
			[ScriptEngineMajorVersion(), ScriptEngineMinorVersion(), ScriptEngineBuildVersion()].join('.');

		console.log(ver);
	};

// #include <Gel.native_module.js>

	startup();

})();