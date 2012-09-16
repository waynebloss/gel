/// <reference path="ref/process.js"/>
/// <reference path="console.js"/>

process.binding.set('evals', (function() {

	var api = process.api('evals');

	function Context() {

	}

	function NodeScript() {

	}
	NodeScript.createContext = function(initSandbox) {
		return new Context();
	};
	NodeScript.runInContext = function(code, context, fileName) {

	};
	NodeScript.runInThisContext = function(code, fileName, displayError) {
//		return api.runInThisContext(code.toString(), fileName.toString(), displayError === true);
		return eval(code.toString());
	};
	NodeScript.runInNewContext = function(code, sandbox, fileName) {
		// CONSIDER: To eval in a sandbox, maybe something like the following,
		// (probably in C# instead of here):
		//
		// (assume var __sandbox = null; in global context)
		// __sandbox = sandbox;
		// code = 'with(__sandbox) {\n__eval=' + code + '\n};'
		//
	};
	NodeScript.prototype.createContext = NodeScript.createContext;
	NodeScript.prototype.runInContext = NodeScript.runInContext;
	NodeScript.prototype.runInThisContext = NodeScript.runInThisContext;
	NodeScript.prototype.runInNewContext = NodeScript.runInNewContext;

	var evals = {
		Context: Context,
		NodeScript: NodeScript
	};

	return evals;
})());