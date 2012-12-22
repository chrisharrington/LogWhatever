String.prototype.startsWith = function (value) {
	if (value == undefined || typeof (value) != "string")
		return false;
	if (value.length == 0)
		return true;
	return this.substring(0, value.length) == value;
};

String.prototype.endsWith = function (value) {
	if (value == undefined || typeof (value) != "string" || value.length > this.length)
		return false;
	if (value.length == 0)
		return true;
	return this.substr(this.length - value.length) == value;
};

String.prototype.padLeft = function (totalLength, value) {
	if (this.length >= totalLength)
		return this;

	var str = this;
	while (str.length < totalLength)
		str = value + str;
	return str;
};

if (!String.prototype.trim) {
	String.prototype.trim = function() {
		return this.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
	};
}