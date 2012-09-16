/// <reference path="ref/process.js"/>

exports.exec = function() {
	var os = require('os');
	console.log('hostname: ' + os.hostname());
	console.log('uptime: ' + os.uptime());
	console.log('totalmem: ' + os.totalmem());
	console.log('freemem: ' + os.freemem());
	console.log('loadavg: ' + os.loadavg());
	console.log('release: ' + os.release());
};