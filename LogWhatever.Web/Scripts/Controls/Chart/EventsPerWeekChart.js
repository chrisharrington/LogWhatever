namespace("LogWhatever.Controls.Chart");

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Constructors */

LogWhatever.Controls.Chart.EventsPerWeekChart = function () { };

LogWhatever.Controls.Chart.EventsPerWeekChart.create = function() {
	return new LogWhatever.Controls.Chart.EventsPerWeekChart();
};

$.extend(LogWhatever.Controls.Chart.EventsPerWeekChart.prototype, LogWhatever.Controls.Chart.ChartDrawer.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Public Methods */

LogWhatever.Controls.Chart.EventsPerWeekChart.prototype.draw = function (container, logName) {
	this._loading(container);
	this._getData(logName).done(function (data) {
		if (data.length == 0) {
			this._empty(container);
			return;
		}

		var categories = new Array();
		var dataPoints = new Array();
		$(data).each(function() {
			categories.push(this.Date);
			dataPoints.push(this.Count);
		});

		var chart = new Highcharts.Chart({
			chart: {
				renderTo: container.attr("id"),
				type: "column"
			},
			xAxis: {
				labels: {
					style: {
						fontFamily: "arial",
						fontSize: "0.8em",
						color: "#888"
					}
				},
				categories: categories
			},
			yAxis: {
				labels: {
					style: {
						fontFamily: "arial",
						fontSize: "0.8em",
						color: "#888"
					}
				},
				title: null
			},
			legend: {
				enabled: false
			},
			plotOptions: {
			},
			title: {
				text: null
			},
			series: [{
				data: dataPoints
				//data: [29.9, 71.5, 106.4, 129.2, 144.0, 176.0, 135.6, 148.5, 216.4, 194.1, 95.6, 54.4]
			}]
		});
	});
};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Controls.Chart.EventsPerWeekChart.prototype._getData = function (logName) {
	return $.get(LogWhatever.Configuration.VirtualDirectory + "api/charts/events-per-week?logName=" + logName).error(function () {
		LogWhatever.Feedback.error("An error has occurred while retrieving data for the events per week chart. Please contact technical support.");
	});
};