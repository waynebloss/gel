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
		// TODO: adding "var __NodeScriptValue = " is a filthy hack to get a return value.
		api.runInThisContext("var __NodeScriptValue = " + code.toString(), fileName.toString(), displayError === true);
		var value = __NodeScriptValue;
		__NodeScriptValue = null;
		return value;
	};
	NodeScript.runInNewContext = function(code, sandbox, fileName) {

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