namespace("LogWhatever.Routers");

LogWhatever.Routers.DashboardRouter = function (params) {
	this._init(params);
};

LogWhatever.Routers.DashboardRouter.create = function (params) {
	return new LogWhatever.Routers.DashboardRouter(params);
};

$.extend(LogWhatever.Routers.DashboardRouter.prototype, LogWhatever.Routers.BaseRouter.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Public Methods */

LogWhatever.Routers.DashboardRouter.prototype._onLoading = function () {
    var deferred = new $.Deferred();

    this._clearSubheader();

	$.when(this._container.find("img").load()).done(function () {
        deferred.resolve();
    });

    return deferred.promise();
};

LogWhatever.Routers.DashboardRouter.prototype._onLoaded = function () {
	this._container.find("#name").clearbox();
	this._container.find(".measurement-name").clearbox();
	this._container.find(".measurement-quantity").clearbox();
	this._container.find(".measurement-units").clearbox();
	this._container.find(".tag-name").clearbox();
	this._container.find("#email-address").focus();
};

