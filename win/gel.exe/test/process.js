/// <reference path="ref/process.js"/>

exports.exec = function() {
	process.nextTick(function() {
		process.exit();
	});
};