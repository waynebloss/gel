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
	return NativeModule._source.hasOwnProperty(id);
};

NativeModule.getSource = function(id) {
	return NativeModule._source[id];
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
	console.log("fn: " + typeof fn);
	//fn(this.exports, NativeModule.require, this, this.filename);
	this.exports.puts = function(xxx) {
		console.log(xxx);
	};

	this.loaded = true;
};

NativeModule.prototype.cache = function() {
	NativeModule._cache[this.id] = this;
};