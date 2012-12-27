namespace("LogWhatever.Routers");

LogWhatever.Routers.DashboardRouter = function (params) {
    this._init(params);
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
	this._container.find("#measurement-name").clearbox();
	this._container.find("#measurement-quantity").clearbox();
	this._container.find("#measurement-units").clearbox();
	this._container.find("#tag-name").clearbox();
	this._container.find("#email-address").focus();
};

LogWhatever.Routers.DashboardRouter.prototype._hookupEvents = function () {
	var me = this;
	this._container.find("#save").click(function () { me._save(); });
};

LogWhatever.Routers.DashboardRouter.prototype._save = function() {
	//this._validate();
	this._sendLogCommand(this._getLogParameters());
};

LogWhatever.Routers.DashboardRouter.prototype._validate = function () {
	this._container.find("input[type='text'].error").removeClass("error");

	var name = this._container.find("#name");
	if (name.clearbox("value") == "") {
		name.addClass("error");
		throw new Error("The name is required.");
	}

	this._validateTime();
};

LogWhatever.Routers.DashboardRouter.prototype._validateMeasurements = function() {
	var panel = this._container.find("div.measurements>div");
	if (panel.find("input.measurement-name").clearbox("value") == "")
		throw new Error("The name for measurements is required.");
	if (panel.find("input.measurement-quantity").clearbox("value") == "")
		throw new Error("The quantity for measurements is required.");
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
		Name: "the name",
		Date: "12/20/2012",
		Time: "7:00 PM",
		Measurements: [
			{ Name: "the measurement name", Quantity: 10.5, Unit: "km" },
			{ Name: "the measurement name 2", Quantity: 15, Unit: "grams" }
		],
		Tags: [
			{ Name: "the first tag", },
			{ Name: "the second tag" }
		]
	};
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