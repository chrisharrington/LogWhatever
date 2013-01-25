namespace("LogWhatever.Controls");

LogWhatever.Controls.Dialog = function (parameters) {
	if (!parameters || !parameters.contentTemplate)
		throw  new Error("No content template was specified for the dialog control.");

	this._init(parameters);
};

/*-------------------------------------------------------------------------------------------------------------------*/
/* Public Methods */

LogWhatever.Controls.Dialog.prototype.load = function (arg) {
    if (typeof arg === "function") {
        this._loadWithData();

        var me = this;
        arg(function (data) {
            me._loadWithData(data);
        });
    }
    else {
        if (!arg)
            arg = {};
        this._loadWithData(arg);
    }
};

LogWhatever.Controls.Dialog.prototype.show = function (load) {
	this._isLoading(load);
	if (!this.menu)
		this.container.find(".actionMenu").hide();
	if (this._isVisible)
		return;

	this._isVisible = true;

	this.container.width(this.width);
	this._setPosition(false);
	this.oldFocusZIndex = this.focus.css("zIndex");

	var maxHeight = $(window).height() - this.vbuffer * 2 - 40;
	this.container.css({ zIndex: this.zindex, maxHeight: maxHeight });
	this.focus.css({ zIndex: this.zindex - 1 });

	this.container.show();
	this.focus.show();
};

LogWhatever.Controls.Dialog.prototype.hide = function () {
	this.container.hide();
	this._isVisible = false;
	if (!this.oldFocusZIndex || this.oldFocusZIndex < 1)
		this.focus.css("zIndex", 0).hide();
	else
		this.focus.css({ zIndex: this.oldFocusZIndex });
	this.onHide();
};

/*-------------------------------------------------------------------------------------------------------------------*/
/* Private Methods */

LogWhatever.Controls.Dialog.prototype._isLoading = function(isLoading) {
	if (isLoading) {
		this.container.find(">div.load").show();
		this.container.find(">div:not(.load)").hide();
	} else {
		this.container.find(">div.load").hide();
		this.container.find(">div:not(.load)").show();
	}
};

LogWhatever.Controls.Dialog.prototype._createDefaults = function() {
	return {
		focus: $("div.focus"),
		onHide: function() {},
		zindex: 20,
		vbuffer: 80,
		closeable: true,
		dialogTemplate: "Dialog/Dialog.html",
		contentTemplateName: "contentTemplate_" + $.uuid(),
		top: true,
		onLoadCompleted: function () {},
		width: 400
	};
};

LogWhatever.Controls.Dialog.prototype._setPosition = function () {
	this.container.css({ left: -10000 }).show();
	var width = this.container.outerWidth();
	this.container.hide();

	this.container.css({ top: this.vbuffer, left: $(window).width() / 2 - width / 2 });
};

LogWhatever.Controls.Dialog.prototype._closeOnEscape = function (e) {
	if (e.keyCode == 27 && this.top)
		this.hide();
};

LogWhatever.Controls.Dialog.prototype._loadTemplates = function () {
	this._loadTemplate(this.dialogTemplate, "dialog");
	if (this.contentTemplate.indexOf)
		this._loadTemplate(this.contentTemplate, this.contentTemplateName);
};

LogWhatever.Controls.Dialog.prototype._loadTemplate = function (location, name) {
	$.ajax({
		async: false,
		url: LogWhatever.Configuration.VirtualDirectory + "Scripts/Templates/" + location,
		success: function (data) { $.template(name, data); },
		error: function () { alert("An error has occurred while loading the template at " + location); }
	});
};

LogWhatever.Controls.Dialog.prototype._getContentTemplateHtml = function (data) {
	if (!data)
		return "";
	if (!this.contentTemplate.indexOf)
		return this.contentTemplate.tmpl(data).html();
	else
		return $.tmpl(this.contentTemplateName, data).html();
};

LogWhatever.Controls.Dialog.prototype._loadWithData = function (data, noshow) {
	this.container.empty();
	if (!data)
		$.tmpl("dialog", {}).appendTo(this.container);
	else
		$.tmpl("dialog", $.extend(data, { content: this._getContentTemplateHtml(data) })).appendTo(this.container);

	var close = this.container.find("div.close");
	var me = this;
	if (this.closeable)
		close.show();
	close.find("img").click(function () {
		me.hide();
	});

	if (this.menu)
		this.menu.build(this.container.find(".actionMenu"));

	if (!noshow)
		this.show(data == undefined);

	if (data)
		this.onLoadCompleted(this.container, data);
};

LogWhatever.Controls.Dialog.prototype._init = function (parameters) {
	if (!this.container || this.container.length == 0) {
		this.container = $("#dialog");
		if (this.container.length == 0) {
			$("body").append($("<div></div>").attr("id", "dialog"));
			this.container = $("#dialog");
		}
	}

	$.extend(this, this._createDefaults(), parameters);

	this.focusZIndex = this.zindex - 1;
	this._loadTemplates();

	if (!this.container.hasClass("dialog"))
		this.container.addClass("dialog");

	if (this.closeable) {
		var me = this;
		$(window).on("keyup", function (e) {
			me._closeOnEscape(e);
		});

		$(document).on("keyup", function (e) {
			me._closeOnEscape(e);
		});
	}
};

LogWhatever.Controls.Dialog.create = function (params) {
    return new LogWhatever.Controls.Dialog(params);
};