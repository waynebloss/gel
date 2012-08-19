/// <reference path="ref/process.js"/>
/// <reference path="console.js"/>

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