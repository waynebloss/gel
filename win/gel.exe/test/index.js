/// <reference path="ref/process.js"/>

exports.exec = function(id) {
	console.log('Test Begin: ' + id);
	require('test.' + id).exec();
	console.log('--');
};