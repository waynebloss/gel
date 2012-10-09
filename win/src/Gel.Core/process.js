/// <reference path="ref/external.js"/>

var process = (function(api) {
	/// <param name="api" type="__gel.ProcessApi">Host Process API.</param>
	delete external;

	var _binding = {};

	function argvFromApi(start, count) {
		/// <returns type="Array"></returns>
		var argv = [];
		for (var i = start; i < count; i++) {
			argv[i] = api.argv(i);
		}
		return argv;
	}

	var argCount = api.argc;

	var _proc = {
		/// <field name="argc" type="Number" integer="true">Count of arguments.</field>
		/// <field name="argv" type="Array">Array of arguments.</field>
		/// <field name="moduleLoadList">Just a shim for now.</field>
		argc: api.argc,
		argv: argvFromApi(0, argCount),
		moduleLoadList: [],
		env: {},
		platform: 'win32',
		mainModule: null,
		_eval: api.evalString,
		_print_eval: api.printEval
	};

	_proc.alert = function(message) {
		/// <summary>Standard alert function.</summary>
		api.alert(message);
	};

	_proc.binding = function(name) {
		return _binding[name];
	};

	_proc.binding.set = function(name, value) {
		_binding[name] = value;
	}

	_proc.cwd = function() {
		return api.cwd();
	}

	_proc.exit = function() {
		this.emit('exit');
		api.exit();
	};

	_proc.api = function(name) {
		return api.getApi(name);
	}

	var nextTickQueue = [];

    _proc.tickCallback = function() {
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
          api.needTickCallback();
        }
        throw e; // process.nextTick error, or 'error' event on first tick
      }
    };

	_proc.nextTick = function(callback) {
		nextTickQueue.push(callback);
		api.needTickCallback();
	};

	return _proc;

	// The external object that is provided by the host program is basically
	// the api object that the Process class wraps.
})(external);

