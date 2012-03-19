var api = process.api('fs');
var fs = exports;
var pathModule = require('path');

fs.statSync = function(path) {
	var isDir = api.IsDirectory(path);
	return {
		IsDirectory: function() {
			return isDir;
		}
	};
};

fs.readFileSync = function(filename, encoding) {
	return api.readFileSync(filename, encoding);
};

fs.realpathSync = function realpathSync(p, cache) {
	p = pathModule.resolve(p);
	if (cache && Object.prototype.hasOwnProperty.call(cache, p)) {
		return cache[p];
	}
	// I don't understand why statSync should be called here,
	// so I'm not doing it. - waynebloss@gmail.com
	//
	// fs.statSync(p);
	//
	if (cache) cache[p] = p;
	return p;
};