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

LogWhatever.Routers.DetailsRouter.prototype._onLoaded = function () {
	this._clearSubheader();
	this._tagResizer.resizeTags(this._container.find("div.tags"));
	this._drawCharts();
};

LogWhatever.Routers.DetailsRouter.prototype._drawCharts = function () {
	var logName = window.location.hash.replace("#/details/", "");
	LogWhatever.Controls.Chart.MeasurementsChart.create().draw(this._container.find("#measurements-chart"), logName);
	LogWhatever.Controls.Chart.TagRatiosChart.create().draw(this._container.find("#tag-ratios-chart"), logName);
};