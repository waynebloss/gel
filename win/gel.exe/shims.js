function engineVer() {
	return ScriptEngine() + '/' + [ScriptEngineMajorVersion(), ScriptEngineMinorVersion(), ScriptEngineBuildVersion()].join('.');
}

if (typeof Array.isArray == 'undefined') {
	Array.isArray = function(o) {
		return (o instanceof Array) ||
        (Object.prototype.toString.apply(o) === '[object Array]');
	};
}

if (typeof {}.__proto__ == 'undefined') {
	Object.prototype.__proto__ = function(source) {
		for (var member in source) {
			this[member] = source[member];
		}
	};
}