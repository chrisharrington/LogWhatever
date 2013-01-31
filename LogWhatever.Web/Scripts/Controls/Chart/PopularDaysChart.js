namespace("LogWhatever.Controls.Chart");

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Constructors */

LogWhatever.Controls.Chart.PopularDaysChart = function () { };

LogWhatever.Controls.Chart.PopularDaysChart.create = function() {
	return new LogWhatever.Controls.Chart.PopularDaysChart();
};

$.extend(LogWhatever.Controls.Chart.PopularDaysChart.prototype, LogWhatever.Controls.Chart.ChartDrawer.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Public Methods */

LogWhatever.Controls.Chart.PopularDaysChart.prototype.draw = function (container, data) {
	var me = this;
	if (data.length == 0) {
		me._empty(container);
		return;
	}

	var series = new Array();
	$(data).each(function() {
		series.push([this.Day, this.Count]);
	});

	var chart = new Highcharts.Chart({
		chart: {
			renderTo: container.attr("id"),
			height: container.parent().height() - container.parent().find("h4").outerHeight(true) - 10,
			backgroundColor: "transparent",
			plotBackgroundColor: "transparent",
			margin: [20, 40, 20, 40],
		},
		xAxis: {
			labels: {
				style: {
					fontFamily: "arial",
					fontSize: "0.8em",
					color: "#888"
				}
			}
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
};