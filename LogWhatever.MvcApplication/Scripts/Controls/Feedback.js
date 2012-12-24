namespace("LogWhatever.Controls");

LogWhatever.Controls.Feedback = function () {};

/*--------------------------------------------------------------------------------------------*/
/* Public Methods */

LogWhatever.Controls.Feedback.prototype.success = function (message) {
	this._showMessage("success", message, 5000);
};

LogWhatever.Controls.Feedback.prototype.error = function (message) {
	this._showMessage("error", message);
};

LogWhatever.Controls.Feedback.prototype.clear = function () {
	if (this._components)
		this._components.panel.slideUp(200);
};

/*--------------------------------------------------------------------------------------------*/
/* Private Methods */

LogWhatever.Controls.Feedback.prototype._showMessage = function (className, message) {
	if (!this._components)
		this._components = this._loadComponents();

	if (this._timeout)
		clearTimeout(this._timeout);

	this._setMessage(message, className);

	if (!this._components.panel.is(":visible"))
		this._components.panel.slideDown(200);
};

LogWhatever.Controls.Feedback.prototype._setMessage = function (message, className) {
	if (this._components.panel.is(":visible")) {
		var me = this;
		this._components.label.fadeOut(150, function () {
			me._components.panel.removeClass("success error").addClass(className);
			me._components.label.fadeIn(150).text(message);
		});
	}
	else {
		this._components.label.text(message);
		this._components.panel.removeClass("success error").addClass(className);
	}
};

LogWhatever.Controls.Feedback.prototype._setMessageAndPosition = function (message) {
	this._components.panel.css({ left: "-1000px" }).show();

	this._components.label.text(message);

	var width = this._components.panel.outerWidth();
	this._components.panel.hide().css({ left: ($(window).width() / 2 - width / 2) + "px" });

	this._components.panel.fadeIn(150);
};

LogWhatever.Controls.Feedback.prototype._loadComponents = function () {
	var components = {};
	if ($("#feedback").length == 0)
		$("body").append($("<div></div>").attr("id", "feedback").append($("<span></span>")));
	components.panel = $("#feedback").click(function () { $(this).fadeOut(200); });
	components.label = components.panel.find("span");
	return components;
};

LogWhatever.Controls.Feedback.prototype._setFadeTimeout = function (milliseconds) {
	if (!milliseconds)
		return;

	var me = this;
	this._timeout = setTimeout(function () {
		me._components.panel.fadeOut(2500);
	}, milliseconds);
};

$(function () {
	LogWhatever.Feedback = new LogWhatever.Controls.Feedback();
});