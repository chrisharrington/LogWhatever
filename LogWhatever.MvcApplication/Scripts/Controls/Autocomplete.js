namespace("LogWhatever.Controls");

(function ($) {
	$.fn.autocomplete = function (parameters) {
		this.each(function () {
			parameters.control = $(this);
			LogWhatever.Controls.Autocomplete.create(parameters);
		});
	};
})(jQuery);

LogWhatever.Controls.Autocomplete = function (parameters) {
	if (!parameters)
		throwArgumentNullException("parameters", "LogWhatever.Controls.Autocomplete", "constructor");
	if (!parameters.onDataRequested)
		throwArgumentNullException("onDataRequested", "LogWhatever.Controls.Autocomplete", "constructor");
	if (!parameters.control)
		throwArgumentNullException("control", "LogWhatever.Controls.Autocomplete", "constructor");

	this._init(parameters);
};

LogWhatever.Controls.Autocomplete.create = function (parameters) {
	return new LogWhatever.Controls.Autocomplete(parameters);
};

/*-------------------------------------------------------------------------------------------------------------------*/
/* Private Methods */

LogWhatever.Controls.Autocomplete.prototype._init = function (parameters) {
	$.extendWithUnderscore(this, parameters, this._createDefaults());

	this._control.attr("autocomplete", "off");
	this._createAutocompletePanel();

	var me = this;
	this._control.keyup(function (e) {
		if (e.keyCode == 27)
			return;
		me._autocomplete($(this).val());
	});
	this._control.dblclick(function () { me._autocomplete($(this).val(), 0); });
	$(window).click(function () { me._panel.hide(); });
	$(window).keyup(function (e) {
		if (e.keyCode == 27)
			me._panel.hide();
	});
};

LogWhatever.Controls.Autocomplete.prototype._createDefaults = function() {
	return {
		interval: 250,
		labelProjection: function (data) { return data; },
		idProjection: function (data) { return data; }
	};
};

LogWhatever.Controls.Autocomplete.prototype._autocomplete = function (value, timeout) {
	if (this._timeout)
		clearTimeout(this._timeout);

	if (timeout == undefined)
		timeout = this._interval;

	var me = this;
	this._timeout = setTimeout(function () {
		me._onDataRequested(value, function (results) {
			if (!results)
				results = new Array();
			me._loadResults(results);
		});
	}, timeout);
};

LogWhatever.Controls.Autocomplete.prototype._loadResults = function(results) {
	var me = this;
	this._panel.empty();
	$(results).each(function(i, result) {
		me._panel.append($("<span></span>").text(me._labelProjection(result)).attr("data-id", me._idProjection(result)).click(function () {
			me._control.attr("data-autocomplete-id", $(this).attr("data-id")).val($(this).text());
			me._panel.hide();
		}));
	});
	if (results.length > 0)
		this._panel.show();
};

LogWhatever.Controls.Autocomplete.prototype._createAutocompletePanel = function() {
	$("body").append(this._panel = $("<div></div>").addClass("autocomplete").attr("id", "autocomplete_" + UUID.generate()));

	var offset = this._control.offset();
	offset.top += this._control.outerHeight();
	this._panel.css(offset);
};