$.extendWithUnderscore = function (dest) {
	if (!dest)
		dest = {};

	for (var i = 1; i < arguments.length; i++) {
		if (!arguments[i])
			continue;

		var curr = arguments[i];
		for (var name in curr) {
			if (!dest["_" + name])
				dest["_" + name] = curr[name];
		}
	}
};