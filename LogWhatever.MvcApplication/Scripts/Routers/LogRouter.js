namespace("LogWhatever.Routers");

LogWhatever.Routers.LogRouter = function (params) {
	this._init(params);
	this._tagResizer = LogWhatever.Controls.TagResizer.create();
};

LogWhatever.Routers.LogRouter.create = function (params) {
	return new LogWhatever.Routers.LogRouter(params);
};

$.extend(LogWhatever.Routers.LogRouter.prototype, LogWhatever.Routers.BaseRouter.prototype);

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Private Methods */

LogWhatever.Routers.LogRouter.prototype._onLoaded = function () {
	this._hookupEvents();
	this._setClearboxes();
};

LogWhatever.Routers.LogRouter.prototype._hookupEvents = function () {
	var me = this;
	this._container.find("div.log").live("click", function () { Finch.navigate("/details/" + $(this).find("h3").text().replace(/ /g, "_")); });
	this._container.find("#add-measurement").click(function () { me._addMeasurement(); });
	this._container.find("#add-tag").click(function() { me._addTag(); });
};

LogWhatever.Routers.LogRouter.prototype._setClearboxes = function() {
	this._container.find("#name").clearbox();
	this._container.find("#new-measurement-name").clearbox();
	this._container.find("#new-measurement-quantity").clearbox();
	this._container.find("#new-measurement-units").clearbox();
	this._container.find("#new-tag-name").clearbox();
};

LogWhatever.Routers.LogRouter.prototype._addMeasurement = function() {
	alert("add measurement");
};

LogWhatever.Routers.LogRouter.prototype._addTag = function() {
	alert("add tag");
};