namespace("LogWhatever.Controls.Chart");

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Constructors */

LogWhatever.Controls.Chart.MeasurementsChart = function () { };

LogWhatever.Controls.Chart.MeasurementsChart.create = function() {
	return new LogWhatever.Controls.Chart.MeasurementsChart();
};

$.extend(LogWhatever.Controls.Chart.MeasurementsChart.prototype, LogWhatever.Controls.Chart.ChartDrawer.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Public Methods */

LogWhatever.Controls.Chart.MeasurementsChart.prototype.draw = function (container, logName) {
	this._loading(container);
	this._getData(logName).done(function (data) {
		var seriesCollection = new Array();
		var axisCollection = new Array();
		$(data).each(function (i, value) {
			axisCollection.push({ min: 0, labels: { enabled: false, style: { fontFamily: "arial", fontSize: "0.8em", color: "#888" }, y: 4 }, title: null, gridLineColor: "#DDD" });
			var series = { name: value.Name, data: new Array(), marker: { symbol: "circle" }, yAxis: i };
			$(value.Data).each(function () {
				series.data.push([new Date(this.Date).getTime(), this.Quantity]);
			});
			seriesCollection.push(series);
		});

		var chart = new Highcharts.Chart({
			chart: {
				renderTo: container.attr("id"),
				margin: [20, 20, 20, 20],
				height: container.parent().height() - container.parent().find("h4").outerHeight(true) - 10,
			},
			title: { text: null },
			xAxis: {
				labels: {
					style: {
						fontFamily: "arial",
						fontSize: "0.8em",
						color: "#888"
					}
				},
				type: "datetime",
				tickPixelInterval: 65
			},
			yAxis: axisCollection,
			plotOptions: {
				series: {
					pointInterval: 24 * 3600 * 1000
				},
				line: {
					shadow: false,
					lineWidth: 3
				}
			},
			legend: {
				enabled: false
			},
			series: seriesCollection
		});
	});
};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Controls.Chart.MeasurementsChart.prototype._getData = function (logName) {
	return $.get(LogWhatever.Configuration.VirtualDirectory + "api/charts/measurements?logName=" + logName).error(function () {
		LogWhatever.Feedback.error("An error has occurred while retrieving data for the measurements chart. Please contact technical support.");
	});
};