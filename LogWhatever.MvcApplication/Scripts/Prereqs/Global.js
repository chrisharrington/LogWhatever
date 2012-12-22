var LogWhatever = window.LogWhatever || {};

$.ajaxSetup({ cache: false });

function namespace(value) {
	var root = window.LogWhatever;
	$(value.replace("LogWhatever.", "").split(".")).each(function (i, part) {
		root[part] = root[part] || {};
		root = root[part];
	});
}

function inspect(o) {
	var string = "";
	for (var name in o)
		string += name + ": " + o[name] + "\n";
	alert(string);
}

function throwArgumentNullException(name, className, method) {
	var text = "The " + name + " parameter is invalid.";
	if (className && method)
		text = className + "." + method + ": " + text;
	else if (className)
		text = className + ": " + text;
	throw text;
}

function assertExists(object, parameter, origin) {
	if (!object || !parameter || parameter == "")
		return;

	if (object[parameter] == undefined)
		throw new Error(origin + ": The " + parameter + " parameter is required.");
}