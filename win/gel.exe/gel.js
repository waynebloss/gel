/// <reference path="ref/process.js"/>
/// <reference path="console.js"/>

// #include <Gel.ddr-ecma5.js>
// #include <Gel.json2.js>
// #include <Gel.process.js>
// #include <Gel.natives.js>
// #include <Gel.console.js>
// #include <Gel.evals.js>

var global;

(function() {

	global = this;

	function startup() {
		var EventEmitter = NativeModule.require('events').EventEmitter;
		process.set__proto__(EventEmitter.prototype);
		process.EventEmitter = EventEmitter; // process.EventEmitter is deprecated

		startup.globalVariables();
		startup.globalTimeouts();
		startup.globalConsole();

		startup.processAssert();
		startup.processNextTick();

		// temp stubs for stdio until startup.processStdio is implemented.
		process.stdout = {write:console.log};
		process.stderr = {write:console.log};

//		startup.processStdio();
//		startup.processKillAndExit();
//		startup.processSignalHandlers();

//		startup.processChannel();

//		startup.resolveArgv0();

		startup.printEngineVer();

		testMain();

		process.on('exit', function() {
			console.log('exiting!!!!!!!!!!!!!!!!!!');
		});

		process.exit();
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
	startup.processNextTick = function() {
		var nextTickQueue = [];

		process._tickCallback = function() {
			var l = nextTickQueue.length;
			if (l === 0) return;

			var q = nextTickQueue;
			nextTickQueue = [];

			try {
				for (var i = 0; i < l; i++) q[i]();
			}
			catch (e) {
				if (i + 1 < l) {
					nextTickQueue = q.slice(i + 1).concat(nextTickQueue);
				}
				if (nextTickQueue.length) {
					process._needTickCallback();
				}
				throw e; // process.nextTick error, or 'error' event on first tick
			}
		};

		process.nextTick = function(callback) {
			nextTickQueue.push(callback);
			process._needTickCallback();
		};
	};

	startup.printEngineVer = function() {
		var ver = ScriptEngine() + '/' + 
			[ScriptEngineMajorVersion(), ScriptEngineMinorVersion(), ScriptEngineBuildVersion()].join('.');

		console.log(ver);
	};

// #include <Gel.native_module.js>

	startup();

	function testMain() {

		var util = NativeModule.require('util');
		util.puts("Hiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii");

		var stream = NativeModule.require('stream');
		console.log("Stream: " + typeof stream);

				function ClassName(name) {
		this.data = name;
	}

		var cn = new ClassName("ClassAa");
				Object.getPrototypeOf(cn).doIt = function() {
		console.log("Hi " + this.data);
	};
		cn.doIt();
		var cn2 = new ClassName("ClassBb");
		cn2.doIt();

		console.log(JSON.stringify(cn));
	
		console.log('Object.defineProperty: ' + typeof Object.defineProperty);
		console.log('Object.defineProperties: ' + typeof Object.defineProperties);

		// Create a user-defined object.
		// To instead use an existing DOM object, uncomment the line below.
		var obj = {};
		//  var obj = window.document;

		// Add a data property to the object.
									Object.defineProperty(obj, "newDataProperty", {
				value: 101,
				writable: true,
				enumerable: true,
				configurable: true
			});

		// Set the property value.
		obj.newDataProperty = 102;
		console.log("Property value: " + obj.newDataProperty);

		// Output:
		//  Property value: 102
	}

})();