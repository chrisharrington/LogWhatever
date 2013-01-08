namespace("LogWhatever.Routers");

LogWhatever.Routers.DetailsRouter = function (params) {
	this._init(params);
};

LogWhatever.Routers.DetailsRouter.create = function (params) {
	return new LogWhatever.Routers.DetailsRouter(params);
};

$.extend(LogWhatever.Routers.DetailsRouter.prototype, LogWhatever.Routers.BaseRouter.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Routers.DetailsRouter.prototype._onLoading = function () {
    var deferred = new $.Deferred();

    this._clearSubheader();

    $.when(this._container.find("img").load()).done(function () {
        deferred.resolve();
    });

    return deferred.promise();
};

LogWhatever.Routers.DetailsRouter.prototype._onLoaded = function () {
	
};