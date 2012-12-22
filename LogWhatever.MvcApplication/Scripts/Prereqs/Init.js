namespace("LogWhatever.Init");

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Constructors */

LogWhatever.Init = function () {
    var me = this;
    $.when(this._getConfigurationData(), this._getLoggedInUser()).done(function () {
        me._createRouters();
        me._hookupMenuLinks();

        $("div.tile.selectable, div.tile.deletable").live("click", function () {
            $(this).toggleClass("selected");
        });
    });
};

LogWhatever.Init.create = function() {
	return new LogWhatever.Init();
};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Init.prototype._createRouters = function () {
	LogWhatever.Routers.WelcomeRouter.create({ html: "templates/welcome", navigation: "welcome" });
    //LogWhatever.Routers.UsersRouter.create({ html: "users/list", navigation: "users", menu: $("#users") });
    //LogWhatever.Routers.LogWhateverRouter.create({ html: "LogWhatever/list", navigation: "home", menu: $("#LogWhatever") });
    //LogWhatever.Routers.ProjectDetailsRouter.create({ html: "projects/:name", navigation: "projects/:name", menu: $("#LogWhatever") });
    //LogWhatever.Routers.ProjectsRouter.create({ html: "projects/list", navigation: "projects", menu: $("#projects") });
    //LogWhatever.Routers.TimeEntryRouter.create({ html: "timeentry/details", navigation: "time-entry", menu: $("#time-entry") });
    //LogWhatever.Routers.MissingRouter.create({ html: "missing", navigation: "missing" });
    //LogWhatever.Routers.LoginRouter.create({ html: "account/login", navigation: "login" });
    //LogWhatever.Routers.ReportsRouter.create({ html: "reports/list", navigation: "reports", menu: $("#reports") });
    Finch.listen();
};

LogWhatever.Init.prototype._hookupMenuLinks = function () {
    var me = this;
    $("#LogWhatever").click(function () { Finch.navigate("/home"); });
    $("#users").click(function () { Finch.navigate("/users"); });
    $("#projects").click(function () { Finch.navigate("/projects"); });
    $("#time-entry").click(function () { Finch.navigate("/time-entry"); });
    $("#reports").click(function() { Finch.navigate("/reports"); });
    $("#log-out").click(function () { me._logOut(); });
};

LogWhatever.Init.prototype._getConfigurationData = function () {
    var deferred = new $.Deferred();
    $.get(LogWhatever.Configuration.VirtualDirectory + "api/configuration").success(function (data) {
        for (var name in data)
            LogWhatever.Configuration[name] = data[name];
    }).complete(function () {
        deferred.resolve();
    });
    return deferred.promise();
};

LogWhatever.Init.prototype._getLoggedInUser = function () {
	return $.get(LogWhatever.Configuration.VirtualDirectory + "api/user/signed-in").success(function (user) {
        LogWhatever.User = user;
        if (!user)
            return;

        var userPanel = $("header>div.user");
        userPanel.find("a:first").text(user.Name);
        userPanel.fadeIn(200);
    });
};

LogWhatever.Init.prototype._logOut = function () {
    return $.post(LogWhatever.Configuration.VirtualDirectory + "account/logout").success(function () {
        Finch.navigate("/login");
    }).error(function () {
        LogWhatever.Feedback.error("An error has occurred while logging you out. Please contact technical support.");
    });
};

$(function () {
	LogWhatever.Init.create();
});

window.onerror = function (e) {
	if (e.toLowerCase().indexOf("highcharts") > -1)
		return;

	//var error = e.replace("Uncaught Error: ", "");
	//LogWhatever.Feedback.error(error);
	alert(e);
};