namespace("LogWhatever.Init");

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Constructors */

LogWhatever.Init = function () {
    var me = this;
    $.when(this._getConfigurationData(), this._getLoggedInUser()).done(function () {
        me._createRouters();
        me._hookupMenuLinks();
        me._createAddLogEntryControl();

	    $("button.logger").click(function() { LogWhatever.Logger.show(); });
	    $("div.tile.selectable, div.tile.deletable").live("click", function () { $(this).toggleClass("selected"); });
	    $(document).keyup(function (e) {
	    	if (e.shiftKey && e.ctrlKey && String.fromCharCode(e.keyCode) == 'L') LogWhatever.Logger.show();
	    	if (e.keyCode == 27) LogWhatever.Logger.hide();
	    });
	    
	    if (window.location.hash == "")
		    Finch.navigate("/dashboard");
    });
};

LogWhatever.Init.create = function() {
	return new LogWhatever.Init();
};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Init.prototype._createRouters = function () {
	LogWhatever.Routers.WelcomeRouter.create({ html: "pages/welcome", navigation: "welcome" });
	LogWhatever.Routers.DashboardRouter.create({ html: "pages/dashboard", navigation: "dashboard" });
	LogWhatever.Routers.DetailsRouter.create({ html: "pages/details/:name", navigation: "details/:name" });
    Finch.listen();
};

LogWhatever.Init.prototype._hookupMenuLinks = function () {
	var me = this;
	$("div.menu>a").click(function () { Finch.navigate("/" + $(this).attr("data-navigation")); });
	$("#sign-out").click(function() { me._signOut(); });
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
        userPanel.find("span:first").text(user.Name);
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

LogWhatever.Init.prototype._createAddLogEntryControl = function() {
	LogWhatever.Logger = new LogWhatever.Controls.Logger({
		container: $("div.container"),
		onLoaded: function () {  }
	});
};

LogWhatever.Init.prototype._signOut = function () {
	LogWhatever.Logger.hide();
	$.post(LogWhatever.Configuration.VirtualDirectory + "api/users/sign-out").success(function() {
		Finch.navigate("/welcome");
		$("header>div.user").fadeOut(200);
	}).error(function() {
		LogWhatever.Feedback.error("An error has occurred while signing out out. Please contact technical support.");
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