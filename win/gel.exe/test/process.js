/// <reference path="ref/process.js"/>

exports.exec = function(testCompletedFn) {
	process.nextTick(function() {
		console.log('process.nextTick callback.');
		testCompletedFn();
	});
};