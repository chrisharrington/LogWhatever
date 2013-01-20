namespace("LogWhatever.Routers");

LogWhatever.Routers.DashboardRouter = function (params) {
	this._init(params);
	this._tagResizer = LogWhatever.Controls.TagResizer.create();
};

LogWhatever.Routers.DashboardRouter.create = function (params) {
	return new LogWhatever.Routers.DashboardRouter(params);
};

$.extend(LogWhatever.Routers.DashboardRouter.prototype, LogWhatever.Routers.BaseRouter.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Routers.DashboardRouter.prototype._hookupEvents = function () {
	this._container.find("div.log").live("click", function () { Finch.navigate("/details/" + $(this).find("h3").text().replace(/ /g, "-")); });
};

LogWhatever.Routers.DashboardRouter.prototype._onLoaded = function () {
	this._tagResizer.resizeTags(this._container.find("div.tags"));
	this._clearSubheader();
	this._hookupEvents();
};

LogWhatever.Routers.DashboardRouter.prototype._setTagWidths = function () {
	this._container.find("div.tags>div").css("width", "auto").css("margin-right", "3px");
	this._contianer.find("div.tags").css({ opacity: 0.01 });

	var me = this;
	this._container.find("div.tags").each(function () {
		var panel = $(this);
		var width = Math.floor(parseFloat(window.getComputedStyle(panel[0]).width.replace("px", "")));
		var labels = new Array();
		var runningWidth = 0;
		panel.find(">div").each(function () {
			runningWidth += $(this).outerWidth(true);
			if (runningWidth >= width) {
				me._setStoredTagWidths(labels, width);
				runningWidth = $(this).outerWidth(true);
				labels = new Array();
			}
			labels.push($(this));
		});

		me._setStoredTagWidths(labels, width);
	});
};

LogWhatever.Routers.DashboardRouter.prototype._getTotalWidth = function(elements) {
	var width = 0;
	$(elements).each(function() {
		width += this.outerWidth(true);
	});
	return width;
};

LogWhatever.Routers.DashboardRouter.prototype._setStoredTagWidths = function (labels, width) {
	if (labels.length == 0)
		return;

	var index = 0;
	labels[labels.length - 1].css("margin-right", "0px");
	var remainingWidth = Math.floor(width - this._getTotalWidth(labels));
	while (remainingWidth > 0) {
		var label = labels[index++];
		label.width(label.width() + 1);
		if (index > labels.length - 1)
			index = 0;
		remainingWidth--;
	}
};