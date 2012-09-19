
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

    var Script = process.binding('evals').NodeScript;
    var runInThisContext = Script.runInThisContext;

    function NativeModule(id) {
	    /// <summary>A minimal module system, which is used to load the core modules
	    /// found in lib/*.js. All core modules are compiled into the binary, so they
	    /// can be loaded faster.</summary>
	    /// <param name="id" type="String">Id of the module.</param>
	    this.filename = id + '.js';
	    this.id = id;
	    this.exports = {};
	    this.loaded = false;
    }

    NativeModule._source = process.binding('natives');
    NativeModule._cache = {};

    NativeModule.require = function(id) {
	    if (id == 'native_module') {
		    return NativeModule;
	    }

	    var cached = NativeModule.getCached(id);
	    if (cached) {
		    return cached.exports;
	    }

	    if (!NativeModule.exists(id)) {
		    throw new Error('No such native module ' + id);
	    }

	    process.moduleLoadList.push('NativeModule ' + id);

	    var nativeModule = new NativeModule(id);

	    nativeModule.compile();
	    nativeModule.cache();

	    return nativeModule.exports;
    };

    NativeModule.getCached = function(id) {
	    return NativeModule._cache[id];
    };

    NativeModule.exists = function(id) {
	    return NativeModule._source.exists(id);
    };

    NativeModule.getSource = function(id) {
	    return NativeModule._source.getSource(id);
    };

    NativeModule.wrap = function(script) {
	    return NativeModule.wrapper[0] + script + NativeModule.wrapper[1];
    };

    NativeModule.wrapper = [
        '(function (exports, require, module, __filename, __dirname) { ',
        '\n});'
      ];

    NativeModule.prototype.compile = function() {
	    var source = NativeModule.getSource(this.id);
	    source = NativeModule.wrap(source);

	    var fn = runInThisContext(source, this.filename, true);
	    fn(this.exports, NativeModule.require, this, this.filename);

	    this.loaded = true;
    };

    NativeModule.prototype.cache = function() {
	    NativeModule._cache[this.id] = this;
    };

    // Wrap a core module's method in a wrapper that will warn on first use
    // and then return the result of invoking the original function. After
    // first being called the original method is restored.
    NativeModule.prototype.deprecate = function(method, message) {
	    var original = this.exports[method];
	    var self = this;
	    var warned = false;
	    message = message || '';

	    Object.defineProperty(this.exports, method, {
		    enumerable: false,
		    value: function() {
			    if (!warned) {
				    warned = true;
				    message = self.id + '.' + method + ' is deprecated. ' + message;

				    var moduleIdCheck = new RegExp('\\b' + self.id + '\\b');
				    if (moduleIdCheck.test(process.env.NODE_DEBUG))
					    console.trace(message);
				    else
					    console.error(message);

				    self.exports[method] = original;
			    }
			    return original.apply(this, arguments);
		    }
	    });
    };

	startup();

})();