namespace("LogWhatever.Controls");

LogWhatever.Controls.Logger = function (parameters) {
	$.extendWithUnderscore(this, parameters, this._createDefaults());

	var me = this;
	this._loadTemplates().done(function () {
		me._loadView();
		me._onLoaded();
		me._hookupEvents();
	});
};

LogWhatever.Controls.Logger.create = function(parameters) {
	return new LogWhatever.Controls.Logger(parameters || {});
};

$.extend(LogWhatever.Controls.Logger.prototype, LogWhatever.Controls.Base.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Public Methods */

LogWhatever.Controls.Logger.prototype.show = function () {
	var me = this;
	this._container.fadeIn(200, function () {
		me._container.find("#email-address").focus();
	});

	this._container.find("#date").val(new Date().toShortDateString()).attr("data-orig", new Date().toShortDateString());
	this._container.find("#name").focus();
};

LogWhatever.Controls.Logger.prototype.hide = function () {
	var me = this;
	this._container.fadeOut(200, function () {
		me._container.find(".error").removeClass("error");
		me._container.find("#name").val("Name?").change();
		me._container.find("#date").val(new Date().toShortDateString());
		me._container.find("#time").val("12:00 AM");
		me._container.find(".cb").clearbox("reset");
		me._container.find("div.added").empty();
	});
};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Controls.Logger.prototype._createDefaults = function() {
	return {
		onLoaded: function () { }
	};
};

LogWhatever.Controls.Logger.prototype._loadView = function () {
	this._container = this._container.find("div.logger");
	
	this._container.find("#name").clearbox();
	this._container.find(".measurement-name").clearbox();
	this._container.find(".measurement-quantity").clearbox();
	this._container.find(".measurement-units").clearbox();
	this._container.find(".tag-name").clearbox();
};

LogWhatever.Controls.Logger.prototype._hookupEvents = function () {
	var me = this;
	this._container.find("#logger-save").click(function () { me._save(); });
	this._container.find("#name").change(function () { me._loadMeasurements($(this).val()); me._loadTags($(this).val()); });
	this._container.find("div.added img.remove-tag").live("click", function () { $(this).parent().fadeOut(200, function () { $(this).remove(); }); });
	this._container.find("#add-measurement").click(function () { me._addMeasurement(); });
	this._container.find("div.added>div").live("click", function () { $(this).fadeOut(200, function () { $(this).remove(); }); });
	this._container.find("#add-tag").click(function () { me._addTag(); });
	this._container.find("#logger-cancel").click(function () { me.hide(); });
};

LogWhatever.Controls.Logger.prototype._addMeasurement = function () {
	this._container.find("input.error").removeClass("error");

	var container = this._container.find("div.measurements>div.new");
	var name = container.find("input.measurement-name");
	var quantity = container.find("input.measurement-quantity");
	var unit = container.find("input.measurement-units");

	if (name.clearbox("value") == "") {
		name.addClass("error").focus();
		throw new Error("The measurement name is required.");
	}

	if (quantity.clearbox("value") == "") {
		quantity.addClass("error").focus();
		throw new Error("The quantity is required.");
	}

	if (isNaN(parseFloat(quantity.clearbox("value")))) {
		quantity.addClass("error").focus();
		throw new Error("The measurement quantity is invalid.");
	}

	this._container.find("div.measurements>div.added").prepend($.tmpl("add-measurement", { name: name.clearbox("value"), quantity: quantity.clearbox("value"), unit: unit.clearbox("value") }));
	this._container.find("div.measurements>div.added>div.new").slideDown(200);
	this._container.find("input.measurement-name, input.measurement-quantity, input.measurement-units").clearbox("reset");
	name.focus();
};

LogWhatever.Controls.Logger.prototype._addTag = function () {
	this._container.find("input.error").removeClass("error");

	var name = this._container.find("input.tag-name");
	if (name.clearbox("value") == "") {
		name.addClass("error").focus();
		throw new Error("The tag name is required.");
	}

	var added = this._container.find("div.tags>div.added");
	added.prepend($.tmpl("add-tag", { name: name.clearbox("value") }));
	added.find(">div.new").slideDown(200).find(">h5");
	name.clearbox("reset").focus();
};

LogWhatever.Controls.Logger.prototype._save = function () {
	this._validate();
	this._sendLogCommand(this._getLogParameters());
};

LogWhatever.Controls.Logger.prototype._validate = function () {
	this._container.find("input[type='text'].error").removeClass("error");
	this._validateName();
	this._validateTime();
	this._validateMeasurements();
};

LogWhatever.Controls.Logger.prototype._validateName = function () {
	var name = this._container.find("#name");
	if (name.clearbox("value") == "") {
		name.addClass("error").focus();
		throw new Error("The name is required.");
	}
};

LogWhatever.Controls.Logger.prototype._validateMeasurements = function () {
	this._container.find("div.measurements>div.added input[type='text']").each(function () {
		var value = $(this).val();
		var error;
		if (value == "")
			error = "The measurement quantity is required.";
		else if (isNaN(parseFloat(value)))
			error = "The measurement quantity is invalid.";

		if (error) {
			$(this).addClass("error").focus();
			throw new Error(error);
		}
	});
};

LogWhatever.Controls.Logger.prototype._validateTag = function () {
	var panel = this._container.find("div.tags>div");
	if (panel.find("input.tag-name").clearbox("value") == "")
		throw new Error("The name for tags is required.");
};

LogWhatever.Controls.Logger.prototype._validateTime = function () {
	var time = this._container.find("#time");
	if (!/^((([0]?[1-9]|1[0-2])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))|(([0]?[0-9]|1[0-9]|2[0-3])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?))$/.test(time.val())) {
		time.addClass("error");
		throw new Error("The time is formatted incorrectly.");
	}
};

LogWhatever.Controls.Logger.prototype._getLogParameters = function () {
	return {
		Name: this._container.find("#name").clearbox("value"),
		Date: this._container.find("#date").val(),
		Time: this._container.find("#time").val(),
		Measurements: this._getLogMeasurementParameters(),
		Tags: this._getLogTagParameters()
	};
};

LogWhatever.Controls.Logger.prototype._getLogMeasurementParameters = function () {
	var measurements = new Array();
	this._container.find("div.measurements>div.added>div").each(function () {
		var panel = $(this);
		measurements.push({ Name: panel.find("h5").text(), Quantity: panel.find("input").val(), Unit: panel.find("span").text() });
	});
	return measurements;
};

LogWhatever.Controls.Logger.prototype._getLogTagParameters = function () {
	var tags = new Array();
	this._container.find("div.tags>div.added>div").each(function () {
		tags.push({ Name: $(this).find("h5").text() });
	});
	return tags;
};

LogWhatever.Controls.Logger.prototype._sendLogCommand = function (parameters) {
	var inputs = this._container.find("input, textarea, select").attr("disabled", true);
	$.post(LogWhatever.Configuration.VirtualDirectory + "api/logs", parameters).success(function () {
		LogWhatever.Feedback.success("Your data has been logged.");
	}).error(function () {
		LogWhatever.Feedback.error("An error has occurred while logging your data. No data was saved. Please contact technical support.");
	}).complete(function () {
		inputs.attr("disabled", false);
	});
};

LogWhatever.Controls.Logger.prototype._loadMeasurements = function (logName) {
	var container = this._container.find("div.measurements>div.added");
	container.find(">div").slideUp(200, function () {
		$(this).remove();
	});

	$.get(LogWhatever.Configuration.VirtualDirectory + "log/measurements", { name: logName }).success(function (html) {
		container.prepend(html);
		container.find("div.new").slideDown(200).find("input").numbersOnly();
	});
};

LogWhatever.Controls.Logger.prototype._loadTags = function (logName) {
	var container = this._container.find("div.tags>div.added");
	container.find(">div").slideUp(200, function () {
		$(this).remove();
	});

	$.get(LogWhatever.Configuration.VirtualDirectory + "log/tags", { name: logName }).success(function (html) {
		container.prepend(html);
		container.find("div.new").slideDown(200);
	});
};

LogWhatever.Controls.Logger.prototype._loadTemplates = function () {
	var deferred = new $.Deferred();
	$.when(
		this._loadTemplate("Scripts/Templates/Log/AddMeasurement.html", "add-measurement"),
		this._loadTemplate("Scripts/Templates/Log/AddTag.html", "add-tag")
	).done(function () {
		deferred.resolve();
	});
	return deferred.promise();
};