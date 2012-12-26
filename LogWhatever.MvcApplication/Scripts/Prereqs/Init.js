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
	LogWhatever.Routers.DashboardRouter.create({ html: "templates/dashboard", navigation: "dashboard" });
    Finch.listen();
};

LogWhatever.Init.prototype._hookupMenuLinks = function () {
	$("div.menu>a").click(function() { Finch.navigate("/" + $(this).attr("data-navigation")); });
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
	return $.get(LogWhatever.Configuration.VirtualDirectory + "api/users/signed-in").success(function (user) {
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

	var error = e.replace("Uncaught Error: ", "");
	LogWhatever.Feedback.error(error);
};