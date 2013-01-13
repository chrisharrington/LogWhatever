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
	container.parent().height(container.parent().width());

	this._getData(logName).done(function (data) {
		var series = new Array();
		$(data).each(function() {
			series.push([this.Name, this.Count]);
		});

		var chart = new Highcharts.Chart({
			chart: {
				animation: false,
				renderTo: container.attr("id"),
				backgroundColor: "transparent",
				plotBackgroundColor: "transparent",
				margin: [-25, 0, 0, 0],
				height: container.height(),
				plotBorderWidth: null,
				plotShadow: false
			},
			title: {
				text: "Tag Ratios",
				style: {
					color: "#888",
					fontSize: "1em",
					fontFamily: "arial"
				}
			},
			plotOptions: {
				pie: {
					allowPointSelect: false,
					dataLabels: { enabled: false },
					borderWidth: 0
				}
			},
			series: [{
				type: 'pie',
				name: 'Browser share',
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