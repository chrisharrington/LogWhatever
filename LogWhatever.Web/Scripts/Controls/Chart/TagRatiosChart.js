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

LogWhatever.Controls.Chart.TagRatiosChart.prototype.draw = function (container, legend, data) {
	var me = this;
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
			height: container.parent().height() - container.parent().find("h4").outerHeight(true) - (legend ? 30 : 10),
			backgroundColor: "transparent",
			plotBackgroundColor: "transparent",
			margin: [20, 0, 20, 0],
		},
		title: { text: null },
		plotOptions: {
			pie: {
				borderWidth: 1,
				animation: false,
				dataLabels: {
					enabled: false
				}
			}
		},
		series: [{
			type: "pie",
			data: series
		}]
	});

	//var labels = new Array();
	//$(chart.series[0].data).each(function() {
	//	labels.push({ label: this.name.toLowerCase(), color: this.color.stops[0][1] });
	//});
	this._drawLegend(legend, chart);
};