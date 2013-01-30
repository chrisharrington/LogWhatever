namespace("LogWhatever.Controls");

LogWhatever.Controls.Base = function() {

};

LogWhatever.Controls.Base.prototype._loadTemplate = function(location, key) {
	return $.get(LogWhatever.Configuration.VirtualDirectory + location).success(function (html) {
		$.template(key, html);
	}).error(function () {
		throw new Error("An error has occurred while retrieving the template at " + location + ". Please contact technical support.");
	})
};