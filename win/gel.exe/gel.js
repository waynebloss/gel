/// <reference path="ref/process.js"/>
/// <reference path="console.js"/>

// #include <Gel.process.js>
// #include <Gel.os.js>
// #include <Gel.natives.js>
// #include <Gel.console.js>
// #include <Gel.evals.js>
// #include <Gel.timer_wrap.js>

var global;

(function() {

	global = this;

	function startup() {
		var EventEmitter = NativeModule.require('events').EventEmitter;
		process.__proto__ = EventEmitter.prototype;
		process.EventEmitter = EventEmitter; // process.EventEmitter is deprecated

		startup.globalVariables();
		startup.globalTimeouts();

		startup.processAssert();
		startup.processStdio();

		startup.printEngineVer();
		
		if (NativeModule.exists('_third_party_main')) {
			// To allow people to extend Node in different ways, this hook allows
			// one to drop a file lib/_third_party_main.js into the build
			// directory which will be executed instead of Node's normal loading.
			process.nextTick(function() {
				NativeModule.require('_third_party_main');
			});

		} else if (process._eval != null) {
			// User passed '-e' or '--eval' arguments to Node.
			var Module = NativeModule.require('module');
			var path = NativeModule.require('path');
			var cwd = process.cwd();

			var module = new Module('eval');
			module.filename = path.join(cwd, 'eval');
			module.paths = Module._nodeModulePaths(cwd);
			var result = module._compile('return eval(process._eval)', 'eval');
			if (process._print_eval) console.log(result);

		} else if (process.argv[1]) {
			// make process.argv[1] into a full path
			var path = NativeModule.require('path');
			process.argv[1] = path.resolve(process.argv[1]);

			var Module = NativeModule.require('module');
			// REMOVEME: nextTick should not be necessary. This hack to get
			// test/simple/test-exception-handler2.js working.
			// Main entry point into most programs:
			process.nextTick(Module.runMain);
		}
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
	
	var assert;
	startup.processAssert = function() {
		// Note that calls to assert() are pre-processed out by JS2C for the
		// normal build of node. They persist only in the node_g build.
		// Similarly for debug().
		assert = process.assert = function(x, msg) {
			if (!x) throw new Error(msg || 'assertion error');
		};
	};

	startup.processStdio = function() {
		// Redirect commonly used stdio functionality to console.
		process.stdout = {write:console.log};
		process.stderr = {write:console.log};
	};

	startup.printEngineVer = function() {
//		var ver = ScriptEngine() + '/' + 
//			[ScriptEngineMajorVersion(), ScriptEngineMinorVersion(), ScriptEngineBuildVersion()].join('.');
		var ver = 'v8';
		console.log(ver);
	};

// #include <Gel.native_module.js>

	startup();

})();