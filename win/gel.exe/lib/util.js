exports.puts = function() {
	for (var i = 0, len = arguments.length; i < len; ++i) {
		console.log(arguments[i]);
	}
};