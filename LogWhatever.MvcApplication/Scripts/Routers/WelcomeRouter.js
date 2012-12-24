﻿namespace("LogWhatever.Routers");

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

LogWhatever.Routers.WelcomeRouter.prototype._onLoaded = function() {
	this._container.find("#email-address").focus();
};

LogWhatever.Routers.WelcomeRouter.prototype._hookupEvents = function () {
	var me = this;
	this._container.find("#sign-in").click(function () { me._signIn(); });
};

LogWhatever.Routers.WelcomeRouter.prototype._signIn = function () {
	var emailAddress = this._container.find("#email-address").val();
	if (!emailAddress || emailAddress == "") {
		this._container.find("#email-address").focus();
		throw new Error("When signing in, your email address is required.");
	}

	var password = this._container.find("#password").val();
	if (!password || password == "") {
		this._container.find("#password").focus();
		throw new Error("When signing in, your password is required.");
	}

	this._sendSignInCommand(emailAddress, password);
};

LogWhatever.Routers.WelcomeRouter.prototype._sendSignInCommand = function (emailAddress, password) {
	$.get(LogWhatever.Configuration.VirtualDirectory + "api/users/sign-in", { emailAddress: emailAddress, password: password }).success(function (user) {
		if (!user)
			LogWhatever.Feedback.error("The email address and password combination you provided is incorrect.");
		else
			LogWhatever.Feedback.clear();
	}).error(function() {
		LogWhatever.Feedback.error("An error has occurred while signing you in. Please contact technical support.");
	});
};