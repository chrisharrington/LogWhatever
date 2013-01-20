namespace("LogWhatever.Routers");

LogWhatever.Routers.BaseRouter = function (params) {
	this._init(params);
};

LogWhatever.Routers.BaseRouter.create = function (params) {
	return new LogWhatever.Routers.BaseRouter(params);
};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Public Methods */

LogWhatever.Routers.BaseRouter.prototype.load = function (parameters) {
    this._container = $("div.content");
    this._subheader = $("div.subheader");

    $("div.menu>.selected").removeClass("selected");
    if (this._menu)
        this._menu.addClass("selected");

    var me = this;
    $.when(this._loadData(parameters), this._container.fadeOutDeferred(200)).done(function (data) {
    	me._container.empty();
    	$.tmpl(me._template, { data: data }).appendTo(me._container);

		me._container.css("opacity", "0.01").show();
		LogWhatever.Controls.Ellipsis.addTitleToShortenedElements();
	    me._onLoaded();
	    me._container.hide().css("opacity", "1");
	    me._container.hide().css("opacity", "1").fadeInDeferred(200, function () {
	    	LogWhatever.Controls.Ellipsis.addTitleToShortenedElements();
	    });
    });
};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Routers.BaseRouter.prototype._init = function (params) {
	assertExists(params, "navigation", "BaseRouter.constructor");

	$.extendWithUnderscore(this, params, this._createDefaults());

	if (!this._navigation.startsWith("/"))
		this._navigation = "/" + this._navigation;

	var me = this;
	Finch.route(this._navigation, function () {
		me.load(arguments[0]);
	});
};

LogWhatever.Routers.BaseRouter.prototype._createDefaults = function () {
	return {
		onLoaded: function () { }
	};
};

LogWhatever.Routers.BaseRouter.prototype._loadData = function (parameters) {
	var deferred = new $.Deferred();
	if (!this._data) {
		deferred.resolve();
		return deferred.promise();
	}

	$.get(LogWhatever.Configuration.VirtualDirectory + this._replacePlaceHoldersWithParameters(this._data, parameters)).complete(function (request) {
    	var status = request.status;
        if (status == 404)
            Finch.navigate("/missing");
        else if (status == 401)
        	Finch.navigate("/welcome");
        else if (status != 200)
        	LogWhatever.Feedback.error("An error has occurred while retrieving the view for the current page. Please contact technical support.");
		deferred.resolve($.parseJSON(request.responseText));
	});

	return deferred.promise();
};

LogWhatever.Routers.BaseRouter.prototype._clearSubheader = function () {
	var me = this;
	this._subheader.find("*").fadeOut(200, function () {
		me._subheader.empty().slideUp(200);
	});
};

LogWhatever.Routers.BaseRouter.prototype._loadSubheader = function (location) {
	var me = this;
	return $.when($.get(LogWhatever.Configuration.VirtualDirectory + "scripts/templates/subheader/" + location).error(function () {
		throw new Error("An error has occurred while retrieving the subheader template located at " + location + ". Please contact technical support.");
	}), this._subheader.find(">*").fadeOut(200)).done(function (result) {
		me._subheader.empty().append($.tmpl(result[0], {}).html()).find(">*").hide();
		me._subheader.slideDown(200, function () {
			me._subheader.find(">*").fadeIn(200);
		});
	});
};

LogWhatever.Routers.BaseRouter.prototype._replacePlaceHoldersWithParameters = function (html, parameters) {
	for (var name in parameters)
		if (html.indexOf(":" + name) > -1)
			html = html.replace(":" + name, parameters[name]);
	return html;
};