namespace("LogWhatever.Controls");

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Constructors */

LogWhatever.Controls.TagResizer = function () { };
LogWhatever.Controls.TagResizer.create = function() {
	return new LogWhatever.Controls.TagResizer();
};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Public Methods */

LogWhatever.Controls.TagResizer.prototype.resizeTags = function (elements) {
	this._setTagWidths(elements);

	var me = this;
	$(window).on("resize", function() {
		me._setTagWidths(elements);
	});
};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Controls.TagResizer.prototype._setTagWidths = function (elements) {
	elements.find(">div").css("width", "auto").css("margin-right", "3px");

	var me = this;
	elements.each(function () {
		var panel = $(this);
		var width = Math.floor(parseFloat(window.getComputedStyle(panel[0]).width.replace("px", "")));
		var labels = new Array();
		var runningWidth = 0;
		panel.find(">div").each(function () {
			runningWidth += $(this).outerWidth(true);
			if (runningWidth >= width) {
				me._setStoredTagWidths(labels, width);
				runningWidth = $(this).outerWidth(true);
				labels = new Array();
			}
			labels.push($(this));
		});
		me._setStoredTagWidths(labels, width);
	});
};

LogWhatever.Controls.TagResizer.prototype._getTotalWidth = function (elements) {
	var width = 0;
	$(elements).each(function () {
		width += this.outerWidth(true);
	});
	return width;
};

LogWhatever.Controls.TagResizer.prototype._setStoredTagWidths = function (labels, width) {
	if (labels.length == 0)
		return;
	
	labels[labels.length - 1].css("margin-right", "0px");

	var totalLabelWidth = 0;
	$(labels).each(function() {
		totalLabelWidth += $(this).outerWidth(true);
	});

	var differential = width - totalLabelWidth;
	var additionalWidthPerLabel = differential / labels.length;
	$(labels).each(function () {
		$(this).outerWidth($(this).outerWidth() + additionalWidthPerLabel);
	});
};