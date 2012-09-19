/// <reference path="ref/process.js"/>

exports.exec = function() {
	var path = require('path');
	var ext = path.extname('boo/boo/boo.js');
	console.log('ext: ' + ext);
};