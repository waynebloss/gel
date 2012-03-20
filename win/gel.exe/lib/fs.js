var api = process.api('fs');
var fs = exports;
var pathModule = require('path');

fs.statSync = function(path) {
	var isDir = api.IsDirectory(path);
	return {
		isDirectory: function() {
			return isDir;
		}
	};
};

fs.readFileSync = function(filename, encoding) {
	return api.readFileSync(filename, encoding);
};

// Node doesn't support symlinks / lstat on windows. Hence realpath is just
// the same as path.resolve that fails if the path doesn't exists.

fs.realpathSync = function realpathSync(p, cache) {
	p = pathModule.resolve(p);
	if (cache && Object.prototype.hasOwnProperty.call(cache, p)) {
		return cache[p];
	}
	if (!api.pathExists(p)) throw Error('Path not found: ' + p);
	if (cache) cache[p] = p;
	return p;
};