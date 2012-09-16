/// <reference path="ref/process.js"/>

var _testCompletedFn;

exports.exec = function(testCompletedFn) {
	_testCompletedFn = testCompletedFn;

	setTimeout(testTimeout, 1000);
};

var _intervalCount = 0;
var _intervalTimer;

function testInterval() {
	console.log("setInterval callback.");
	if (++_intervalCount > 2) {
		clearInterval(_intervalTimer);
		_testCompletedFn();
	}
}

function testTimeout() {
	console.log("setTimeout callback.");
	
	_intervalTimer = setInterval(testInterval, 1000);
}