namespace("LogWhatever.Routers.Welcome");

LogWhatever.Routers.Welcome.Register = function (params) {
	$.extendWithUnderscore(this, params);

	assertExists(this, "_container", "LogWhatever.Routers.Welcome.Register");

	this._hookupEvents();
};

LogWhatever.Routers.Welcome.Register.create = function (params) {
	return new LogWhatever.Routers.Welcome.Register(params);
};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Public Methods */

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Routers.Welcome.Register.prototype._hookupEvents = function() {
	var me = this;
	this._container.find("#register").click(function() { me._register(); });
};

LogWhatever.Routers.Welcome.Register.prototype._register = function() {
	this._validate();
};

LogWhatever.Routers.Welcome.Register.prototype._validate = function() {
	this._container.find(".error").removeClass("error");

	var params = this._readParameters();
	this._validateRequiredParameter(params.name, "name");
	this._validateRequiredParameter(params.email, "email");
	this._validateRequiredParameter(params.password, "password");
	this._validateRequiredParameter(params.confirmPassword, "confirmed password");
	this._validatePasswords(params.password, params.confirmPassword);

	this._sendRegisterCommand(params);
};

LogWhatever.Routers.Welcome.Register.prototype._validateRequiredParameter = function(control, name) {
	if (control.val() == "") {
		control.addClass("error").focus();
		throw new Error("When registering, the " + name + " is required.");
	}
};

LogWhatever.Routers.Welcome.Register.prototype._validatePasswords = function(password, confirmPassword) {
	if (password.val() != confirmPassword.val()) {
		password.addClass("error");
		confirmPassword.addClass("error");
		throw new Error("Your passwords don't match.");
	}
};

LogWhatever.Routers.Welcome.Register.prototype._readParameters = function() {
	return {
		name: this._container.find("#register-name"),
		email: this._container.find("#register-email-address"),
		password: this._container.find("#register-password"),
		confirmPassword: this._container.find("#register-confirm-password")
	};
};

LogWhatever.Routers.Welcome.Register.prototype._sendRegisterCommand = function(params) {
	var inputs = this._container.find("input, select, textarea").attr("disabled", true);

	params = { name: params.name.val(), email: params.email.val(), password: params.password.val() };
	$.get(LogWhatever.Configuration.VirtualDirectory + "api/users/registration", params).success(function () {
		Finch.navigate("/dashboard");
		$("header>div.user").fadeIn(200);
		$("#user-name").text(LogWhatever.User.Name);
	}).error(function() {
		LogWhatever.Feedback.error("An error has occurred while performing your registration. Please contact technical support.");
	}).complete(function() {
		inputs.attr("disabled", false);
	});
};