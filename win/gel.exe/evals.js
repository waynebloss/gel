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
		api.runInThisContext(code.toString(), fileName.toString(), displayError === true);
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

// HACK: The following code is a hack to retrieve a value from runInThisContext.

var __definedVal = null;

function __define(value) {
	__definedVal = value;
};

function __d$(voidArg) {
	/// <summary>Returns the __definedVal for a call to an evals exported function.</summary>
	/// <param name="voidArg">Place here the call to a NodeScript method such as NodeScript.runInNewContext.</param>
	var rval = __definedVal;
	__definedVal = null;
	return rval;
}