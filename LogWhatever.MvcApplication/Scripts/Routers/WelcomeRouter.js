namespace("LogWhatever.Routers");

LogWhatever.Routers.WelcomeRouter = function (params) {
    this._init(params);
};

LogWhatever.Routers.WelcomeRouter.create = function (params) {
	return new LogWhatever.Routers.WelcomeRouter(params);
};

$.extend(LogWhatever.Routers.WelcomeRouter.prototype, LogWhatever.Routers.BaseRouter.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Public Methods */

LogWhatever.Routers.WelcomeRouter.prototype._onLoading = function () {
    var deferred = new $.Deferred();

    this._clearSubheader();
    this._hookupEvents();

    $.when(this._container.find("img").load()).done(function () {
        deferred.resolve();
    });

    return deferred.promise();
};

LogWhatever.Routers.WelcomeRouter.prototype._hookupEvents = function () {
	this._container.find("div.LogWhatever-project-tile>h1").die("click").live("click", function () {
		Finch.navigate("/projects/" + $(this).text().replace( / /g , "_"));
	});
};