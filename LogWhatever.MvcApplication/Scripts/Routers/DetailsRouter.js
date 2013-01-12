namespace("LogWhatever.Routers");

LogWhatever.Routers.DetailsRouter = function (params) {
	this._init(params);
	this._tagResizer = LogWhatever.Controls.TagResizer.create();
};

LogWhatever.Routers.DetailsRouter.create = function (params) {
	return new LogWhatever.Routers.DetailsRouter(params);
};

$.extend(LogWhatever.Routers.DetailsRouter.prototype, LogWhatever.Routers.BaseRouter.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Routers.DetailsRouter.prototype._onLoading = function () {
    var deferred = new $.Deferred();

    this._clearSubheader();

    $.when(this._container.find("img").load()).done(function () {
        deferred.resolve();
    });

    return deferred.promise();
};

LogWhatever.Routers.DetailsRouter.prototype._onLoaded = function () {
	this._tagResizer.resizeTags(this._container.find("div.tags"));
	this._drawCharts();
};

LogWhatever.Routers.DetailsRouter.prototype._drawCharts = function () {
	//var logName = window.location.hash()
	var logName = "swimming";
	LogWhatever.Controls.Chart.Measurements.create().measurements(this._container.find("#measurements-chart"), logName);
};