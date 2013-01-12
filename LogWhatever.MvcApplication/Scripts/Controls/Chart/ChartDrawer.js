namespace("LogWhatever.Controls.Chart");

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Constructors */

LogWhatever.Controls.Chart.ChartDrawer = function () { };
LogWhatever.Controls.Chart.ChartDrawer.create = function() {
	return new LogWhatever.Controls.Chart.ChartDrawer();
};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Controls.Chart.ChartDrawer.prototype._buildChart = function (container, type) {
	var chart = new JSChart(container.attr("id"), type);
	chart.setSize(container.width(), container.parent().height());
	return chart;
};

LogWhatever.Controls.Chart.ChartDrawer.prototype._loading = function(isLoading) {

};