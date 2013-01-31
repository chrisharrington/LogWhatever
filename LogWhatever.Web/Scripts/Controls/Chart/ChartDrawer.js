﻿namespace("LogWhatever.Controls.Chart");

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Constructors */

LogWhatever.Controls.Chart.ChartDrawer = function () { };
LogWhatever.Controls.Chart.ChartDrawer.create = function() {
	return new LogWhatever.Controls.Chart.ChartDrawer();
};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Controls.Chart.ChartDrawer.prototype._empty = function(container, message) {
	if (!message)
		message = "No data.";

	container.empty().append($.tmpl("chart-empty", { message: message }));
	var parent = container.parent();
	var totalHeight = parent.height();
	var headerHeight = parent.find("h4").outerHeight(true);
	container.height(totalHeight - headerHeight);

	var label = container.find("span");
	label.css("margin-top", ((totalHeight - headerHeight) / 2 - label.height() / 2) + "px");
};