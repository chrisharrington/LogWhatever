namespace("LogWhatever.Routers");

LogWhatever.Routers.LogRouter = function (params) {
	this._init(params);
	this._tagResizer = LogWhatever.Controls.TagResizer.create();
};

LogWhatever.Routers.LogRouter.create = function (params) {
	return new LogWhatever.Routers.LogRouter(params);
};

$.extend(LogWhatever.Routers.LogRouter.prototype, LogWhatever.Routers.BaseRouter.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Routers.LogRouter.prototype._onLoaded = function () {
	this._hookupEvents();
	this._setClearboxes();
	this._container.find("#name").focus();
};

LogWhatever.Routers.LogRouter.prototype._hookupEvents = function () {
	var me = this;
	this._container.find("div.log").live("click", function () { Finch.navigate("/details/" + $(this).find("h3").text().replace(/ /g, "_")); });
	this._container.find("#new-measurement-name, #new-measurement-quantity, #new-measurement-units").enter(function () { me._addMeasurement(); });
	this._container.find("#new-tag-name").enter(function () { me._addTag(); });
	this._container.find("#name").change(function () { me._loadMeasurements($(this).val()); me._loadEvents($(this).val()); });
	this._container.find("#save").click(function () { me._save(); });
	this._container.find("div.new div.floating-tag").die("click").live("click", function() { $(this).slideUp(200, function() { $(this).remove(); }); });
};

LogWhatever.Routers.LogRouter.prototype._setClearboxes = function() {
	this._container.find("#name").clearbox();
	this._container.find("#new-measurement-name").clearbox();
	this._container.find("#new-measurement-quantity").clearbox();
	this._container.find("#new-measurement-units").clearbox();
	this._container.find("#new-tag-name").clearbox();
};

LogWhatever.Routers.LogRouter.prototype._addMeasurement = function (beforeSave) {
	this._container.find("input.error").removeClass("error");

	var container = this._container.find("div.measurements>div.add");
	var name = container.find("#new-measurement-name");
	var quantity = container.find("#new-measurement-quantity");
	var unit = container.find("#new-measurement-units");
	var groupId = UUIDjs.create();
	this._validateMeasurement(name, quantity);

	this._container.find("div.measurements>div.list").prepend($.tmpl("log-add-measurement", { GroupId: groupId, Name: name.clearbox("value"), Quantity: parseFloat(quantity.clearbox("value")).toFixed(2), Unit: unit.clearbox("value") }).hide());
	name.clearbox("reset");
	quantity.clearbox("reset");
	unit.clearbox("reset");

	var added = this._container.find("div.measurements>div.list>div.measurement:first");
	if (beforeSave)
		container.slideUp(200, function () {
			added.slideDown(200);
		});
	else
		added.slideDown(200);
	name.focus();
};

LogWhatever.Routers.LogRouter.prototype._validateMeasurement = function(name, quantity) {
	this._container.find("div.measurements div.list input.measurement-name").each(function () {
		if ($(this).val().toString().toLowerCase() == name.val().toString().toLowerCase())
			throw new Error("The measurement name already exists.");
	});

	if (quantity.clearbox("value") == "") {
		quantity.addClass("error").focus();
		throw new Error("The quantity is required.");
	}

	if (isNaN(parseFloat(quantity.clearbox("value")))) {
		quantity.addClass("error").focus();
		throw new Error("The measurement quantity is invalid.");
	}
};

LogWhatever.Routers.LogRouter.prototype._addTag = function () {
	this._container.find("input.error").removeClass("error");

	var name = this._container.find("input.tag-name");
	var nameValue = name.clearbox("value");
	if (nameValue == "") {
		name.addClass("error").focus();
		throw new Error("The tag name is required.");
	}

	var me = this;
	var added = this._container.find("div.tags>div.list");
	added.slideUp(200, function () {
		added.css("opacity", "0.01").show().prepend($.tmpl("log-add-tag", { name: nameValue }));
		me._tagResizer.resizeTags(added);
		added.hide().css("opacity", "1").slideDown(200);
	});
	name.clearbox("reset").focus();
};

LogWhatever.Routers.LogRouter.prototype._save = function () {
	this._validate();
	this._addMeasurementAndTagIfNecessary();
	this._sendLogCommand(this._getLogParameters());
};

LogWhatever.Routers.LogRouter.prototype._addMeasurementAndTagIfNecessary = function () {
	var measurementQuantity = this._container.find("#new-measurement-quantity");
	if (measurementQuantity.clearbox("value") != "")
		this._addMeasurement(true);
	
	var tagName = this._container.find("#new-tag-name");
	if (tagName.clearbox("value") != "")
		this._addTag();
};

LogWhatever.Routers.LogRouter.prototype._validate = function () {
	this._container.find("input[type='text'].error").removeClass("error");
	this._validateName();
	this._validateTime();
};

LogWhatever.Routers.LogRouter.prototype._validateName = function () {
	var name = this._container.find("#name");
	if (name.clearbox("value") == "") {
		name.addClass("error").focus();
		throw new Error("The name is required.");
	}
};

LogWhatever.Routers.LogRouter.prototype._validateTime = function () {
	var time = this._container.find("#time");
	if (!/^((([0]?[1-9]|1[0-2])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?( )?(AM|am|aM|Am|PM|pm|pM|Pm))|(([0]?[0-9]|1[0-9]|2[0-3])(:|\.)[0-5][0-9]((:|\.)[0-5][0-9])?))$/.test(time.val())) {
		time.addClass("error");
		throw new Error("The time is formatted incorrectly.");
	}
};

LogWhatever.Routers.LogRouter.prototype._getLogParameters = function () {
	return {
		Name: this._container.find("#name").clearbox("value"),
		Date: this._container.find("#date").val(),
		Time: this._container.find("#time").val(),
		Measurements: this._getLogMeasurementParameters(),
		Tags: this._getLogTagParameters()
	};
};

LogWhatever.Routers.LogRouter.prototype._getLogMeasurementParameters = function () {
	var measurements = new Array();
	this._container.find("div.measurements>div.list>div").each(function () {
		var panel = $(this);
		measurements.push({ GroupId: panel.find("input[type='hidden']").val(), Name: panel.find("input.measurement-name").val(), Quantity: panel.find("input.measurement-quantity").val(), Unit: panel.find("input.measurement-units").val() });
	});
	return measurements;
};

LogWhatever.Routers.LogRouter.prototype._getLogTagParameters = function () {
	var tags = new Array();
	this._container.find("div.tags>div.list>div").each(function () {
		tags.push({ Name: $(this).text() });
	});
	return tags;
};

LogWhatever.Routers.LogRouter.prototype._sendLogCommand = function (parameters) {
	var me = this;
	var inputs = this._container.find("input, textarea, select").attr("disabled", true);
	$.post(LogWhatever.Configuration.VirtualDirectory + "api/logs", parameters).success(function () {
		//LogWhatever.Feedback.success("Your data has been logged.");
		me._loadMeasurements(parameters.Name, true);
		me._loadEvents(parameters.Name);
	}).error(function () {
		LogWhatever.Feedback.error("An error has occurred while logging your data. No data was saved. Please contact technical support.");
	}).complete(function () {
		inputs.attr("disabled", false);	
	});
};

LogWhatever.Routers.LogRouter.prototype._loadMeasurements = function (logName, afterSave) {
	var container = this._container.find("div.measurements>div.list");
	var add = this._container.find("div.measurements>div.add");
	$.when(this._getMeasurements(logName)).done(function (measurements) {
		if (afterSave) {
			add.slideUpDeferred(200);
		} else if (container.find(">*").length > 0 && measurements.length == 0) {
			container.slideUpDeferred(200, function() {
				container.empty();
				add.slideDownDeferred(200);
			});
		} else if (container.find(">*").length == 0 && measurements.length == 0) {
			add.slideDownDeferred(200);
		} else if (container.find(">*").length > 0 && measurements.length > 0) {
			container.slideUpDeferred(200, function() {
				container.hide().empty();
				$.tmpl("log-add-measurement", measurements).appendTo(container);
				container.find("input.measurement-name, input.measurement-units").attr("readonly", true).css("color", "#AAA");
				container.slideDownDeferred(200);
			});
		} else if (container.find(">*").length == 0 && measurements.length > 0) {
			add.slideUpDeferred(200, function () {
				container.hide().empty();
				$.tmpl("log-add-measurement", measurements).appendTo(container);
				container.find("input.measurement-name, input.measurement-units").attr("readonly", true).css("color", "#AAA");
				container.slideDownDeferred(200);
			});
		}
	});
};

LogWhatever.Routers.LogRouter.prototype._getMeasurements = function (logName) {
	var deferred = new $.Deferred();
	if (!logName || logName.trim() == "")
		deferred.resolve([]);
	else
		$.get(LogWhatever.Configuration.VirtualDirectory + "api/measurements/log", { logName: logName }).success(function (measurements) {
			deferred.resolve(measurements);
		}).error(function() {
			LogWhatever.Feedback.error("An error has occurred while retrieving the previous measurements for the log. Please contact technical support.");
		});

	return deferred.promise();
};

LogWhatever.Routers.LogRouter.prototype._loadEvents = function (logName) {
	var me = this;
	var container = this._container.find("div.previous");

	$.when(container.fadeOutDeferred(200), this._getEvents(logName)).done(function (result, events) {
		container.empty();
		$.tmpl("log-events", { data: events }).appendTo(container);
		container.css("opacity", "0.01").show();
		me._tagResizer.resizeTags(container.find("div.tags"));
		container.hide().css("opacity", "1");
		container.fadeInDeferred(200);
	});
};

LogWhatever.Routers.LogRouter.prototype._getEvents = function(logName) {
	var deferred = new $.Deferred();
	if (!logName || logName.trim() == "")
		deferred.resolve([]);
	else
		$.get(LogWhatever.Configuration.VirtualDirectory + "api/events/log", { logName: logName }).error(function() {
			LogWhatever.Feedback.error("An error has occurred while retrieving the previous event list. Please contact technical support.");
		}).success(function(events) {
			var first = [], second = [], third = [];
			$(events).each(function(i) {
				if (i % 3 == 0)
					first.push(this);
				else if (i % 3 == 1)
					second.push(this);
				else
					third.push(this);
			});
			deferred.resolve([first, second, third]);
		});
	return deferred.promise();
};