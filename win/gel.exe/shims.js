
Array.isArray = function(o) {
	return (o instanceof Array) ||
        (Object.prototype.toString.apply(o) === '[object Array]');
};

Object.prototype.__proto__ = function(source) {
	for (var member in source) {
		this[member] = source[member];
	}
};