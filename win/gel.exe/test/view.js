/// <reference path="ref/process.js"/>

var _testCompletedFn;

exports.exec = function(testCompletedFn) {
	_testCompletedFn = testCompletedFn;

	require('view').alert('Hello, World!!');

	_testCompletedFn();
};