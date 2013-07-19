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

LogWhatever.Controls.Chart.MeasurementsChart.prototype.draw = function (container, measurements) {
	var seriesCollection = new Array();
	var axisCollection = new Array();
		
	if (measurements.length == 0) {
		this._empty(container);
		return;
	}

	var colors = ['#4572A7', '#AA4643', '#89A54E', '#80699B', '#3D96AE', '#DB843D', '#92A8CD', '#A47D7C', '#B5CA92'];

	$(measurements).each(function (i, value) {
		axisCollection.push({ labels: { style: { fontFamily: "arial", fontSize: "0.8em", color: colors[i] }, y: 4 }, title: null, gridLineColor: "#DDD" });
		var series = { name: value[0].Name, data: new Array(), marker: { symbol: "circle" }, yAxis: i };
		$(value).each(function () {
			series.data.push([new Date(this.Date).getTime(), this.Quantity]);
		});
		seriesCollection.push(series);
	});
		
	var chart = new Highcharts.Chart({
		chart: {
			renderTo: container.attr("id"),
			margin: [20, 20, 20, measurements.length * 40],
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
				pointInterval: 24 * 3600 * 1000,
				animation: false
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
};