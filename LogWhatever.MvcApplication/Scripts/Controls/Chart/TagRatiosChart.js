namespace("LogWhatever.Controls.Chart");

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Constructors */

LogWhatever.Controls.Chart.TagRatiosChart = function () { };

LogWhatever.Controls.Chart.TagRatiosChart.create = function() {
	return new LogWhatever.Controls.Chart.TagRatiosChart();
};

$.extend(LogWhatever.Controls.Chart.TagRatiosChart.prototype, LogWhatever.Controls.Chart.ChartDrawer.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Public Methods */

LogWhatever.Controls.Chart.TagRatiosChart.prototype.draw = function (container, logName) {
	Highcharts.getOptions().colors = Highcharts.map(Highcharts.getOptions().colors, function (color) {
		return {
			radialGradient: { cx: 0.5, cy: 0.3, r: 0.7 },
			stops: [
				[0, color],
				[1, Highcharts.Color(color).brighten(-0.3).get('rgb')] // darken
			]
		};
	});

	this._loading(container);

	var me = this;
	this._getData(logName).done(function (data) {
		if (data.length == 0) {
			me._empty(container);
			return;
		}

		var series = new Array();
		$(data).each(function() {
			series.push([this.Name, this.Count]);
		});

		var chart = new Highcharts.Chart({
			chart: {
				renderTo: container.attr("id"),
				height: container.parent().height() - container.parent().find("h4").outerHeight(true) - 10,
				backgroundColor: "transparent",
				plotBackgroundColor: "transparent",
				margin: [20, 40, 20, 40],
			},
			title: { text: null },
			plotOptions: {
				pie: {
					borderWidth: 1
				}
			},
			series: [{
				type: "pie",
				data: series
			}]
		});
	});
};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Controls.Chart.TagRatiosChart.prototype._getData = function (logName) {
	return $.get(LogWhatever.Configuration.VirtualDirectory + "api/charts/tag-ratios?logName=" + logName).error(function () {
		LogWhatever.Feedback.error("An error has occurred while retrieving data for the measurements chart. Please contact technical support.");
	});
};