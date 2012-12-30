namespace("LogWhatever.Routers");

LogWhatever.Routers.DashboardRouter = function (params) {
	this._init(params);
	this._loadTemplates();
};

LogWhatever.Routers.DashboardRouter.create = function (params) {
	return new LogWhatever.Routers.DashboardRouter(params);
};

$.extend(LogWhatever.Routers.DashboardRouter.prototype, LogWhatever.Routers.BaseRouter.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Public Methods */

LogWhatever.Routers.DashboardRouter.prototype._onLoading = function () {
    var deferred = new $.Deferred();

    this._clearSubheader();
    this._hookupEvents();

	$.when(this._container.find("img").load()).done(function () {
        deferred.resolve();
    });

    return deferred.promise();
};

LogWhatever.Routers.DashboardRouter.prototype._onLoaded = function () {
	this._container.find("#name").clearbox();
	this._container.find(".measurement-name").clearbox();
	this._container.find(".measurement-quantity").clearbox();
	this._container.find(".measurement-units").clearbox();
	this._container.find(".tag-name").clearbox();
	this._container.find("#email-address").focus();
};

LogWhatever.Routers.DashboardRouter.prototype._hookupEvents = function () {
	var me = this;
	this._container.find("#save").click(function () { me._save(); });
	this._container.find("#name").change(function () { me._loadMeasurements($(this).val()); me._loadTags($(this).val()); });
	this._container.find("div.added img.remove-tag").live("click", function () { $(this).parent().fadeOut(200, function () { $(this).remove(); }); });
	this._container.find("#add-measurement").click(function () { me._addMeasurement(); });
	this._container.find("div.added img.remove-measurement").live("click", function () { $(this).parent().fadeOut(200, function () { $(this).remove(); }); });
	this._container.find("#add-tag").click(function() { me._addTag(); });
};

LogWhatever.Routers.DashboardRouter.prototype._addMeasurement = function () {
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
		throw new Error("The ")
	}
	
	if (isNaN(parseFloat(quantity.clearbox("value")))) {
		quantity.addClass("error").focus();
		throw new Error("The measurement quantity is invalid.");
	}

	this._container.find("div.measurements>div.added").prepend($.tmpl("add-measurement", { name: name.clearbox("value"), quantity: quantity.clearbox("value"), unit: unit.clearbox("value") }));
	this._container.find("div.measurements>div.added>div.new").slideDown(200);
};

LogWhatever.Routers.DashboardRouter.prototype._save = function() {
	this._validate();
	this._sendLogCommand(this._getLogParameters());
};

LogWhatever.Routers.DashboardRouter.prototype._validate = function () {
	this._container.find("input[type='text'].error").removeClass("error");
	this._validateName();
	this._validateTime();
	this._validateMeasurements();
};

LogWhatever.Routers.DashboardRouter.prototype._validateName = function() {
	var name = this._container.find("#name");
	if (name.clearbox("value") == "") {
		name.addClass("error");
		throw new Error("The name is required.");
	}
};

LogWhatever.Routers.DashboardRouter.prototype._validateMeasurements = function() {
	this._container.find("div.measurements>div.added input[type='text']").each(function() {
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

LogWhatever.Routers.DashboardRouter.prototype._validateTag = function() {
	var panel = this._container.find("div.tags>div");
	if (panel.find("input.tag-name").clearbox("value") == "")
		throw new Error("The name for tags is required.");
};

LogWhatever.Routers.DashboardRouter.prototype._validateTime = function () {
	var time = this._container.find("#time");
	if (!/^((([0]?[1-9]|1[0-2])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))|(([0]?[0-9]|1[0-9]|2[0-3])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?))$/.test(time.val())) {
		time.addClass("error");
		throw new Error("The time is formatted incorrectly.");
	}
};

LogWhatever.Routers.DashboardRouter.prototype._getLogParameters = function() {
	return {
		Name: this._container.find("#name").clearbox("value"),
		Date: this._container.find("#date").val(),
		Time: this._container.find("#time").val(),
		Measurements: this._getLogMeasurementParameters(),
		Tags: this._getLogTagParameters()
	};
};

LogWhatever.Routers.DashboardRouter.prototype._getLogMeasurementParameters = function() {
	var measurements = new Array();
	this._container.find("div.measurements>div.added>div").each(function() {
		var panel = $(this);
		measurements.push({ Name: panel.find("h5").text(), Quantity: panel.find("input").val(), Unit: panel.find("span").text() });
	});
	return measurements;
};

LogWhatever.Routers.DashboardRouter.prototype._getLogTagParameters = function() {
	var tags = new Array();
	this._container.find("div.tags>div.added>div").each(function() {
		tags.push({ Name: $(this).find("h5").text() });
	});
	return tags;
};

LogWhatever.Routers.DashboardRouter.prototype._sendLogCommand = function(parameters) {
	var inputs = this._container.find("input, textarea, select").attr("disabled", true);

	$.post(LogWhatever.Configuration.VirtualDirectory + "api/logs", parameters).success(function() {
		LogWhatever.Feedback.success("Your data has been logged.");
	}).error(function() {
		LogWhatever.Feedback.error("An error has occurred while logging your data. No data was saved. Please contact technical support.");
	}).complete(function() {
		inputs.attr("disabled", false);
	});
};

LogWhatever.Routers.DashboardRouter.prototype._loadMeasurements = function (logName) {
	var container = this._container.find("div.measurements>div.added");
	container.find(">div").slideUp(200, function() {
		$(this).remove();
	});

	$.get(LogWhatever.Configuration.VirtualDirectory + "log/measurements", { name: logName }).success(function(html) {
		container.prepend(html);
		container.find("div.new").slideDown(200).find("input").numbersOnly();
	});
};

LogWhatever.Routers.DashboardRouter.prototype._loadTags = function (logName) {
	var container = this._container.find("div.tags>div.added");
	container.find(">div").slideUp(200, function () {
		$(this).remove();
	});

	$.get(LogWhatever.Configuration.VirtualDirectory + "log/tags", { name: logName }).success(function (html) {
		container.prepend(html);
		container.find("div.new").slideDown(200);
	});
};

LogWhatever.Routers.DashboardRouter.prototype._loadTemplates = function() {
	$.get(LogWhatever.Configuration.VirtualDirectory + "Templates/Log/Measurement.html").success(function(html) {
		$.template("add-measurement", html);
	});

	$.get(LogWhatever.Configuration.VirtualDirectory + "Templates/Log/Tag.html").success(function(html) {
		$.template("add-tag", html);
	});
};

LogWhatever.Routers.DashboardRouter.prototype._addTag = function () {
	this._container.find("input.error").removeClass("error");
	
	var name = this._container.find("input.tag-name");
	if (name.clearbox("value") == "") {
		name.addClass("error").focus();
		throw new Error("The tag name is required.");
	}

	this._container.find("div.tags>div.added").prepend($.tmpl("add-tag", { name: name.clearbox("value") }));
	this._container.find("div.tags>div.added>div.new").slideDown(200);
};