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
//		var test = NativeModule.require('test.index')
//		test.exec('os');
//		test.exec('path');
//		test.exec('buffer');
//		test.exec('timers');
//		test.exec('process');
		
		if (NativeModule.exists('_third_party_main')) {
			// To allow people to extend Node in different ways, this hook allows
			// one to drop a file lib/_third_party_main.js into the build
			// directory which will be executed instead of Node's normal loading.
			process.nextTick(function() {
				NativeModule.require('_third_party_main');
			});

//		} else if (process.argv[1] == 'debug') {
//			// Start the debugger agent
//			var d = NativeModule.require('_debugger');
//			d.start();

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

			// If this is a worker in cluster mode, start up the communiction
			// channel.
//			if (process.env.NODE_UNIQUE_ID) {
//			var cluster = NativeModule.require('cluster');
//			cluster._setupWorker();
//			}

			var Module = NativeModule.require('module');
			// REMOVEME: nextTick should not be necessary. This hack to get
			// test/simple/test-exception-handler2.js working.
			// Main entry point into most programs:
			process.nextTick(Module.runMain);

		}
//		else {
//			var Module = NativeModule.require('module');

//			// If stdin is a TTY.
//			if (NativeModule.require('tty').isatty(0)) {
//			// REPL
//			var repl = Module.requireRepl().start('> ', null, null, true);

//			} else {
//			// Read all of stdin - execute it.
//			process.stdin.resume();
//			process.stdin.setEncoding('utf8');

//			var code = '';
//			process.stdin.on('data', function(d) {
//				code += d;
//			});

//			process.stdin.on('end', function() {
//				new Module()._compile(code, '[stdin]');
//			});
//			}
//		}
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