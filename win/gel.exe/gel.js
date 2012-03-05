﻿/// <reference path="ref/process.js"/>
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

	startup.printEngineVer = function() {
		var ver = ScriptEngine() + '/' + 
			[ScriptEngineMajorVersion(), ScriptEngineMinorVersion(), ScriptEngineBuildVersion()].join('.');

		console.log(ver);
	};

// #include <Gel.native_module.js>

	startup();

})();

function testMain() {

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