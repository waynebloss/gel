/// <reference path="ref/process.js"/>

exports.exec = function() {
	//setTimeout(testTimeout, 1000);
	var y = 0;
	var t = setInterval(function() {
		console.log("setInterval callback.");
		if (++y > 2) {
			clearInterval(t);
			process.exit();
		}
	}, 1000);
};

function testTimeout()
{
	console.log("TIMEOUT!!");
	process.exit();
}