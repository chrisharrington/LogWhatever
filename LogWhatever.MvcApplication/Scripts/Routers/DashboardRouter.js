namespace("LogWhatever.Routers");

LogWhatever.Routers.DashboardRouter = function (params) {
	this._init(params);
	this._tagResizer = LogWhatever.Controls.TagResizer.create();

	var me = this;
	$(window).on("resize", function () {
		me._setTagWidths();
	});
};

LogWhatever.Routers.DashboardRouter.create = function (params) {
	return new LogWhatever.Routers.DashboardRouter(params);
};

$.extend(LogWhatever.Routers.DashboardRouter.prototype, LogWhatever.Routers.BaseRouter.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Routers.DashboardRouter.prototype._onLoading = function () {
    var deferred = new $.Deferred();

    this._clearSubheader();
    //this._showLoggerIfEmpty();
	this._hookupEvents();

    $.when(this._container.find("img").load()).done(function () {
        deferred.resolve();
    });

    return deferred.promise();
};

LogWhatever.Routers.DashboardRouter.prototype._hookupEvents = function () {
	this._container.find("div.log").live("click", function () { Finch.navigate("/details/" + $(this).find("h3").text().replace(/ /g, "_")); });
};

LogWhatever.Routers.DashboardRouter.prototype._onLoaded = function () {
	this._setLogTileHeights();
	this._tagResizer.resizeTags(this._container.find("div.tags"));
};

LogWhatever.Routers.DashboardRouter.prototype._setLogTileHeights = function () {
	var localMax = 0;
	var me = this;
	this._container.find("div.log").each(function (index) {
		if (index % 5 == 0 && localMax > 0) {
			me._container.find("div.log.local-max").css("min-height", localMax + "px").removeClass("local-max");
			localMax = 0;
		}

		$(this).addClass("local-max");
		localMax = Math.max($(this).height(), localMax);
	});

	this._container.find("div.log.local-max>div").css("min-height", localMax + "px").removeClass("local-max");
};

LogWhatever.Routers.DashboardRouter.prototype._showLoggerIfEmpty = function() {
	if (this._container.find("div.log").length == 0)
		LogWhatever.Logger.show();
};

LogWhatever.Routers.DashboardRouter.prototype._setTagWidths = function () {
	this._container.find("div.tags>div").css("width", "auto").css("margin-right", "3px");

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