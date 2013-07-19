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

LogWhatever.Controls.Chart.EventsPerWeekChart.prototype.draw = function (container, data) {
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
			series: {
				animation: false
			}
		},
		title: {
			text: null
		},
		series: [{
			data: dataPoints
		}]
	});
};