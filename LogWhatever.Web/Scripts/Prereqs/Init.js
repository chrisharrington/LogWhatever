namespace("LogWhatever.Init");

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Constructors */

LogWhatever.Init = function () {
    var me = this;
    $.when(this._loadTemplates(), this._getConfigurationData(), this._getLoggedInUser()).done(function () {
        me._createRouters();
        me._hookupMenuLinks();
	    me._setChartColours();

	    $("div.tile.selectable, div.tile.deletable").live("click", function () { $(this).toggleClass("selected"); });
	    
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
	LogWhatever.Routers.WelcomeRouter.create({ template: "pages-welcome", navigation: "welcome" });
	LogWhatever.Routers.DashboardRouter.create({ template: "pages-dashboard", data: "api/dashboard", navigation: "dashboard" });
	LogWhatever.Routers.DetailsRouter.create({ template: "pages-details", data: "api/details?name=:name", navigation: "details/:name" });
	LogWhatever.Routers.LogRouter.create({ template: "pages-log", navigation: "log-some-stuff" });
    Finch.listen();
};

LogWhatever.Init.prototype._hookupMenuLinks = function () {
	var me = this;
	$("header [data-navigation]").click(function () { Finch.navigate("/" + $(this).attr("data-navigation")); });
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
        userPanel.find("#user-account>span").text(user.Name);
        userPanel.fadeIn(200);
	}).error(function () {
	    LogWhatever.Feedback.error("An error has occurred while retrieving the logged in user data.");
    });
};

LogWhatever.Init.prototype._logOut = function () {
    return $.post(LogWhatever.Configuration.VirtualDirectory + "account/logout").success(function () {
        Finch.navigate("/login");
    }).error(function () {
        LogWhatever.Feedback.error("An error has occurred while logging you out. Please contact technical support.");
    });
};

LogWhatever.Init.prototype._signOut = function () {
	$.post(LogWhatever.Configuration.VirtualDirectory + "api/users/sign-out").success(function() {
		Finch.navigate("/welcome");
		$("header>div.user").fadeOut(200);
	}).error(function() {
		LogWhatever.Feedback.error("An error has occurred while signing you out. Please contact technical support.");
	});
};

LogWhatever.Init.prototype._loadTemplates = function() {
	return $.get(LogWhatever.Configuration.VirtualDirectory + "api/templates").success(function (templates) {
		$(templates).each(function () {
			$.template(this.Name, this.Html);
		});
	}).error(function() {
		LogWhatever.Feedback.error("An error has occurred while retrieving the template list. Please contact technical support.");
	});
};

LogWhatever.Init.prototype._setChartColours = function () {
	Highcharts.getOptions().colors = Highcharts.map(Highcharts.getOptions().colors, function(color) {
		return {
			radialGradient: { cx: 0.5, cy: 0.3, r: 0.7 },
			stops: [
				[0, color],
				[1, Highcharts.Color(color).brighten(-0.3).get('rgb')]
			]
		};
	});
};

$(function () {
	LogWhatever.Init.create();
});

window.onerror = function (e) {
	if (e.toLowerCase().indexOf("highcharts") > -1)
		return;

	var error = e.replace("Uncaught Error: ", "");
	if (!LogWhatever.Feedback)
		alert(error);
	else
		LogWhatever.Feedback.error(error);
};

function blah(date) {
	return new Date(date).toShortDateString();
}