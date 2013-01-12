namespace("LogWhatever.Controls.Chart");

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Constructors */

LogWhatever.Controls.Chart.Measurements = function () { };

LogWhatever.Controls.Chart.Measurements.create = function() {
	return new LogWhatever.Controls.Chart.Measurements();
};

$.extend(LogWhatever.Controls.Chart.Measurements.prototype, LogWhatever.Controls.Chart.ChartDrawer.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Public Methods */

LogWhatever.Controls.Chart.Measurements.prototype.measurements = function (container, logName) {
	var me = this;
	this._getData(logName).done(function (data) {
		var chart = me._buildChart(container, "line");
		chart.setDataArray(new Array(['unit', 20], ['unit two', 10], ['unit three', 30], ['other unit', 10], ['last unit', 30]));
		chart.setTitle("Measurements Over Time");
		chart.draw();
	});
};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Controls.Chart.Measurements.prototype._getData = function (logName) {
	return $.get(LogWhatever.Configuration.VirtualDirectory + "api/charts/measurements?logName=" + logName).error(function() {
		LogWhatever.Feedback.error("An error has occurred while retrieving data for the measurements chart. Please contact technical support.");
	});
};