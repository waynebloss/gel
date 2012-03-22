﻿/// <reference path="ref/external.js"/>

var process = (function(api) {
	/// <param name="api" type="__gel.ProcessApi">Host Process API.</param>
	delete external;

	var _binding = {};

	function Process() {
		/// <field name="argc" type="Number" integer="true">Count of arguments.</field>
		/// <field name="argv" type="Array">Array of arguments.</field>
		/// <field name="moduleLoadList">Just a shim for now.</field>
		this.argc = api.argc;
		this.argv = argvFromApi(0, this.argc);
		this.moduleLoadList = [];
		this.env = {};
		this.platform = 'win32';
		this.mainModule = null;
	}

	Process.prototype.alert = function(message) {
		/// <summary>Standard alert function.</summary>
		api.alert(message);
	};

	Process.prototype.binding = function(name) {
		return _binding[name];
	};

	Process.prototype.binding.set = function(name, value) {
		_binding[name] = value;
	}

	Process.prototype.cwd = function() {
		return api.cwd();
	}

	Process.prototype.exit = function() {
		this.emit('exit');
		api.exit();
	};

	Process.prototype.api = function(name) {
		return api.getApi(name);
	}

	function argvFromApi(start, count) {
		/// <returns type="Array"></returns>
		var argv = [];
		for (var i = start; i < count; i++) {
			argv[i] = api.argv(i);
		}
		return argv;
	}

	var nextTickQueue = [];

    api.tickCallback = function() {
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

	Process.prototype.nextTick = function(callback) {
		nextTickQueue.push(callback);
		api.needTickCallback();
	};

	return new Process();

	// The external object that is provided by the host program is basically
	// the api object that the Process class wraps.
})(external);

