(function (api) {
	delete external;

	// private backing field for the binding method.
	var _bindings = {};

	function Process() {
		this.argc = api.argc;
		this.argv = argvFromApi(0, this.argc);
	}

	Process.prototype.alert = function (message) {
		api.alert(message);
	};

	Process.prototype.binding = function (type) {
		return _bindings[type];
	};

	Process.prototype.exit = function () {
		api.exit();
	};

	function argvFromApi(start, count) {
		var argv = [];
		for (var i = start; i < count; i++) {
			argv[i] = api.argv(i);
		}
		return argv;
	}

	return new Process();

	// The external object that is provided by IE is the api object
	// that the Process class wraps.
})(external)
