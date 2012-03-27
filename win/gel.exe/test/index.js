/// <reference path="ref/process.js"/>

function exec(id, cb) {
	console.log('Test Begin: ' + id);
	console.log('--');
	
	var test = require('./' + id + '.js');

	if (test.exec) {
		if (cb) {
			test.exec(function() {
				console.log('--');
				cb();
			});
			return;
		}
		test.exec();
	}
	console.log('--');
};

exec('assert');
exec('buffer');
exec('os');
exec('path');
exec('process', function() {
	exec('string_decoder');
	exec('test_module');
	exec('timers', function() {
		process.exit();
	});
});