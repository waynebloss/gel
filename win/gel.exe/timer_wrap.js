/// <reference path="ref/process.js"/>

process.binding.set('timer_wrap', (function() {
	
	var api = process.api('timers');
	var count = 0;
	var active = {};

	function Timer() {
		
		this._id = ++count;
		active[this._id] = this;
		this.ontimeout = function() {
			console.log("Timer.ontimeout default.");
		};
	}
	
	Timer.prototype.close = function() {
		api.close(this._id);
	};

	Timer.prototype.start = function(timeMs, repeatTimeMs) {
		api.start(this._id, timeMs, repeatTimeMs);
	};
	
	Timer.prototype.stop = function() {
		api.stop(this._id);
	};
	
	Timer.prototype.getRepeat = function() {
		return api.getRepeat(this._id);
	};
	
	Timer.prototype.setRepeat = function(value) {
		api.setRepeat(this._id, value);
	};
	
	Timer.prototype.again = function() {
		api.again(this._id);
	};
	
	function callback(id) {
		var timer = active[id];
		delete active[id];
		timer.ontimeout();
	}

	return {
		Timer: Timer,
		callback: callback
	};

})());