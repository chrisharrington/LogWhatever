
Array.prototype.take = function (length) {
	var result = new Array();
	length = Math.min(this.length, length);
	for (var i = 0; i < length; i++)
		result.push(this[i]);
	return result;
};

Array.prototype.toObject = function (projection) {
	var result = {};
	for (var i = 0; i < this.length; i++)
		result[this[i][projection]] = this[i];
	return result;
};