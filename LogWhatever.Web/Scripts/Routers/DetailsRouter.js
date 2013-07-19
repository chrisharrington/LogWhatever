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

	this._setChartsLoading();

	var me = this;
	$.when(this._getChartData(logName)).done(function (data) {
		LogWhatever.Controls.Chart.MeasurementsChart.create().draw(me._container.find("#measurements-chart"), data.measurements);
		LogWhatever.Controls.Chart.TagRatiosChart.create().draw(me._container.find("#tag-ratios-chart"), me._container.find("#tag-ratios-legend"), data.tagRatios);
		LogWhatever.Controls.Chart.EventsPerWeekChart.create().draw(me._container.find("#events-per-week-chart"), data.eventsOverTime);
		LogWhatever.Controls.Chart.PopularDaysChart.create().draw(me._container.find("#popular-days-chart"), data.popularDays);
	});
};

LogWhatever.Routers.DetailsRouter.prototype._getChartData = function(logName) {
	return $.get(LogWhatever.Configuration.VirtualDirectory + "api/charts", { logName: logName }).error(function() {
		LogWhatever.Feedback.error("An error has occurred while retrieving the chart data. Please contact technical support.");
	});
};

LogWhatever.Routers.DetailsRouter.prototype._setChartsLoading = function() {
	this._container.find("div.chart").each(function() {
		var container = $(this).empty().append($.tmpl("chart-loading"));
		var parent = container.parent();
		var totalHeight = parent.height();
		var legend = $(this).parent().find(".legend");
		if (legend.length > 0)
			totalHeight -= legend.height();
		var headerHeight = parent.find("h4").outerHeight(true);
		container.height(totalHeight - headerHeight);

		var imageContainer = container.find(">div>div");
		imageContainer.css("margin-top", ((totalHeight - headerHeight) / 2 - imageContainer.height() / 2) + "px");
	});
};