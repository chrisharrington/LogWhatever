namespace("LogWhatever.Routers");

LogWhatever.Routers.DashboardRouter = function (params) {
	this._init(params);
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
	this._showLoggerIfEmpty();

    $.when(this._container.find("img").load()).done(function () {
        deferred.resolve();
    });

    return deferred.promise();
};

LogWhatever.Routers.DashboardRouter.prototype._onLoaded = function () {
	this._setLogTileHeights();
};

LogWhatever.Routers.DashboardRouter.prototype._setLogTileHeights = function () {
	var localMax;
	var me = this;
	this._container.find("div.log").each(function (index) {
		if (index % 4 == 0 && localMax > 0) {
			me._container.find("div.log.local-max").height(localMax).removeClass("local-max");
			localMax = 0;
		}

		$(this).addClass("local-max");
		localMax = $(this).height();
	});

	this._container.find("div.log.local-max").height(localMax).removeClass("local-max");
};

LogWhatever.Routers.DashboardRouter.prototype._showLoggerIfEmpty = function() {
	if (this._container.find("div.log").length == 0)
		LogWhatever.Logger.show();
};