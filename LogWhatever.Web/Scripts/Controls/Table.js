namespace("LogWhatever.Controls");

LogWhatever.Controls.Table = function (params) {
	if ((!params.func || typeof (params.func) != "function") && (!params.dataSource || !params.dataSource.url))
		throw new Error("No data function or data source was specified when creating the table.");

	this._init(params);
};

/*-------------------------------------------------------------------------------------------------------------------*/
/* Public Methods */

LogWhatever.Controls.Table.prototype.load = function (length, expression, direction, params) {
	this._length = length;

	var me = this;

	this._currentPage = 1;
	this._currentSort = expression;
	this._currentDirection = direction;
	this._currentFilter = "";
	this._params = params || {};

	this._func(this._currentPage, this._currentSort, this._currentDirection, this._currentFilter, function (data) {
		me._setupFirstPage(data, length);
	});
};

LogWhatever.Controls.Table.prototype.setFilter = function (filter) {
	this._loadData(this._currentPage, this._currentSort, this._currentDirection, filter);
};

LogWhatever.Controls.Table.prototype.setCount = function (length) {
	if (length < 0)
		throw new Error("The length must be a non-negative integer.");

	this._length = length;
	this._setPagerText(length);
};

LogWhatever.Controls.Table.prototype.exportData = function () {
	if (!this._dataSourceUrl)
		throw new Error("Export functionality is not supported by this table.");

	var dataSource = this._dataSourceUrl;
	var dataParameters = "";
	for (var attrname in this._dataSourceParameters)
		dataParameters += "&" + attrname + "=" + this._dataSourceParameters[attrname];
	$.download(LogWhatever.CurrentVirtualDirectory + "ExcelExport/Export" + "?controllerAction=" + dataSource + "&pageSize=100000&pageNumber=1&sortDirection=" + this._currentDirection + "&sortColumn=" + this._currentSort + dataParameters, { filter: this._currentFilter });
};

LogWhatever.Controls.Table.prototype.refresh = function (count) {
	this._cached = {};
	if (count)
		this._length = count;
	this._setPage(1);
	this._loadPagerEvents(count);
};

/*-------------------------------------------------------------------------------------------------------------------*/
/* Event Handlers */

LogWhatever.Controls.Table.prototype._onHeaderClicked = function (header) {
	if (header.hasClass("nosort"))
		return;

	var expression = header.attr("name");
	var isAscending = this._sort[expression] ? this._sort[expression] : false;
	this._sort[expression] = !isAscending;
	this._loadData(this._currentPage, expression, isAscending ? "asc" : "desc", this._currentFilter);

	this._components.container.find("thead td.asc, thead td.desc").removeClass("asc").removeClass("desc");
	header.addClass(isAscending ? "asc" : "desc");
};

/*-------------------------------------------------------------------------------------------------------------------*/
/* Private Methods */

LogWhatever.Controls.Table.prototype._setupFirstPage = function (data, length) {
	this._sanitizeDateData(data);

	if (!this._mapping)
		this._loadMapping(data);

	$.tmpl("table_" + this._name, $.extend({ mapping: this._toKeyValueList(this._mapping), pageSize: this._options.pageSize, sort: this._sort, isEmpty: data.length == 0 }, this._params)).appendTo(this._components.container);

	if (data && data.length > 0) {
		this._onFirstDataLoaded();
		this._bindRowData(this, data, length, this._currentSort, this._currentDirection);
		if (this._onRowClicked) {
			var me = this;
			this._components.container.find("tbody tr").addClass("clickable").click(function (e) {
				me._onRowClicked($(this).data("data"), $(this), e);
			});
		}
	}
};

LogWhatever.Controls.Table.prototype._init = function (params) {
	if (params.isTest)
		return;

	this._name = params.name;
	if (!this._name)
		this._name = $.uuid();
	this._options = params.options;
	this._mapping = params.mapping;
	this._template = params.template;
	this._tableTemplate = params.tableTemplate;
	this._currentPage = 1;
	this._length = 0;
	this._isLoaded = false;
	this._cached = {};
	this._sort = params.sort || {};

	this._onRowClicked = params.onRowClicked;
	this._onDataLoaded = params.onDataLoaded || function () { };
	this._onFirstDataLoaded = params.onFirstDataLoaded || function () { };

	this._currentSort = this._sort.sort;
	this._currentDirection = this._sort.direction;
	this._currentFilter = "";

	if (params.dataSource) {
		this._dataSourceUrl = params.dataSource.url;
		this._dataSourceParameters = params.dataSource.parameters;
	}

	if (this._dataSourceUrl && !params.func)
		this._handleDataSource();
	else
		this._func = this._wrapCallback(params.func);

	this._loadComponents();
	this._sanitizeOptions();
	this._loadTemplate();
};

LogWhatever.Controls.Table.prototype._loadComponents = function () {
	this._components = {};
	this._components.container = this._options.container;
};

/*
* Derives a mapping from column to data using the bound data as a guide. Property names of data objects will be used
* as column headers.
* @data The bound data.
*/
LogWhatever.Controls.Table.prototype._loadMapping = function (data) {
	this._mapping = {};
	for (var name in data[0])
		this._mapping[name] = name;
};

LogWhatever.Controls.Table.prototype._sanitizeOptions = function () {
	if (!this._options)
		this._options = { };
	if (!this._options.pageSize)
		this._options.pageSize = 20;
	if (!this._options.prefetch)
		this._options.prefetch = 3;
	if (!this._options.timeout)
		this._options.timeout = 20;
};

LogWhatever.Controls.Table.prototype._setPagerText = function (length) {
	var pager = this._components.container.find("tfoot div");

	var page = this._currentPage;
	this._numberOfPages = Math.ceil(length / this._options.pageSize);
	pager.find("a:eq(1)").showOrHide(page > 1);
	pager.find("a:eq(2)").text(page - 2).showOrHide(page >= 3);
	pager.find("a:eq(3)").text(page - 1).showOrHide(page >= 2);
	pager.find("a:eq(4)").text(page);
	pager.find("a:eq(5)").text(page + 1).showOrHide(page <= this._numberOfPages - 1);
	pager.find("a:eq(6)").text(page + 2).showOrHide(page <= this._numberOfPages - 2);
	pager.find("a:eq(7)").showOrHide(page < this._numberOfPages);

	pager.show();
};

LogWhatever.Controls.Table.prototype._getDefaultSortExpression = function () {
	for (var name in this._mapping)
		return name;
};

LogWhatever.Controls.Table.prototype._loadPagerEvents = function (length) {
	var pager = this._components.container.find("tfoot div");
	var numberOfPages = Math.ceil(length / this._options.pageSize);
	var me = this;
	pager.find("a:eq(0)").die("click").live("click", function () { me._setPage(1); });
	pager.find("a:eq(1)").die("click").live("click", function () { me._setPage(me._currentPage - 1); });
	pager.find("a:eq(2)").die("click").live("click", function () { me._setPage(me._currentPage - 2); });
	pager.find("a:eq(3)").die("click").live("click", function () { me._setPage(me._currentPage - 1); });
	pager.find("a:eq(5)").die("click").live("click", function () { me._setPage(me._currentPage + 1); });
	pager.find("a:eq(6)").die("click").live("click", function () { me._setPage(me._currentPage + 2); });
	pager.find("a:eq(7)").die("click").live("click", function () { me._setPage(me._currentPage + 1); });
	pager.find("a:eq(8)").die("click").live("click", function () { me._setPage(numberOfPages); });
};

LogWhatever.Controls.Table.prototype._setPage = function (page) {
	this._loadData(page, this._currentSort, this._currentDirection, this._currentFilter);
};

LogWhatever.Controls.Table.prototype._loadData = function (page, sort, direction, filter) {
	var me = this;
	me._currentPage = page;
	me._currentSort = sort;
	me._currentDirection = direction;
	me._currentFilter = filter;

	if (!me._components || !me._components.tablecover)
		me._setTableCoverDimensions();

	me._components.tablecover.show();
	me._func(page, sort, direction, filter, function (data) {
		$.tmpl("row_" + me._name, $.extend({ mapping: me._mapping, data: data, isClickable: me._onRowClicked != undefined }, me._params)).appendTo(me._components.container.find("tbody").empty());
		$(me._components.container.find("tbody tr").each(function (i, row) {
			$(row).data("data", data[i]);
		}));
		me._setCachedData(page, sort, direction, filter, data);
		me._setPagerText(me._length);
		me._components.tablecover.hide();
		me._onDataLoaded(me._components.container);
		me._setTableCoverDimensions();
		if (me._onRowClicked)
			me._components.container.find("tbody tr").click(function (e) {
				me._onRowClicked($(this).data("data"), $(this), e);
			});
	});

	me._loadPrefetchData(page, sort, direction, filter);
};

LogWhatever.Controls.Table.prototype._loadPrefetchData = function (page, sort, direction, filter) {
	var me = this;
	for (var i = page + 1; i <= page + me._options.prefetch; i++) {
		if (i > me._numberOfPages || me._options[(page + i) + "|" + sort + "|" + direction + "|" + filter])
			continue;

		var closure = function (index) {
			me._func(index, sort, direction, filter, function (data) {
				me._setCachedData(index, sort, direction, filter, data);
			});
		};
		closure(i);
	}
};

LogWhatever.Controls.Table.prototype._setTableCoverDimensions = function () {
	var cover = this._components.container.find("div.tablecover");
	var table = this._components.container.parent();
	cover.css({ width: table.outerWidth(), height: table.outerHeight() });
	this._components.tablecover = cover;
};

LogWhatever.Controls.Table.prototype._wrapCallback = function (func) {
	var me = this;
	return function (page, sort, direction, filter, callback) {
		var cached = me._cached[me._deriveCacheKey(page, sort, direction, filter)];
		if (cached && cached.expiry > new Date())
			callback(cached.data);
		else
			func(page, sort, direction, filter, callback);
	};
};

LogWhatever.Controls.Table.prototype._setCachedData = function (page, sort, direction, filter, data) {
	var query = this._deriveCacheKey(page, sort, direction, filter);
	var expiry;
	if (this._cached[query] && this._cached[query].expiry > new Date())
		expiry = this._cached[query].expiry;
	else {
		expiry = new Date();
		expiry.setSeconds(expiry.getSeconds() + this._options.timeout);
	}
	this._cached[query] = { expiry: expiry, data: data };
};

LogWhatever.Controls.Table.prototype._loadTemplate = function () {
	var me = this;
	$.ajax({
		async: false,
		url: this._tableTemplate ? this._tableTemplate : LogWhatever.CurrentVirtualDirectory + "Scripts/Templates/Table/Table.html",
		success: function (data) { $.template("table_" + me._name, data); }
	});
	$.ajax({
		async: false,
		url: this._template ? this._template : LogWhatever.CurrentVirtualDirectory + "Scripts/Templates/Table/Row.html",
		success: function (data) { $.template("row_" + me._name, data); }
	});
};

LogWhatever.Controls.Table.prototype._toKeyValueList = function(obj) {
	var list = new Array();
	for (var name in obj)
		list.push({ key: name, value: obj[name] });
	return list;
};

LogWhatever.Controls.Table.prototype._sanitizeDateData = function (data) {
	$(data).each(function (i, obj) {
		for (var name in obj) {
			if (obj[name] && obj.indexOf && obj[name].indexOf("/Date(") > -1)
				obj[name] = Date.parseJSON(obj[name]);
		}
	});
};

LogWhatever.Controls.Table.prototype._bindRowData = function (me, data, length, expression, direction) {
	$.tmpl("row_" + me._name, $.extend({ mapping: me._mapping, data: data, isClickable: me._onRowClicked != undefined }, me._params)).appendTo(me._components.container.find("tbody"));

	if (!me._isLoaded) {
		me._loadPagerEvents(length);
		me._components.container.find("thead td").die("click").live("click", function () { me._onHeaderClicked.call(me, $(this)); });
		me._setTableCoverDimensions();
	}

	me._setPagerText(length);
	me._isLoaded = true;
	me._onDataLoaded(me._components.container);
	me._setCachedData(me._currentPage, expression, direction, me._currentFilter, data);
	me._loadPrefetchData(me._currentPage, expression, direction, me._currentFilter);

	$(me._components.container.find("tbody tr").each(function (i, row) {
		$(row).data("data", data[i]);
	}));
};

LogWhatever.Controls.Table.prototype._handleDataSource = function () {
	var me = this;
	this._func = this._wrapCallback(function (page, sort, direction, filter, callback) {
		var data = { pageSize: me._options.pageSize, pageNumber: page, sortColumn: sort, sortDirection: direction, filter: filter || {} };
		$.extend(data, me._dataSourceParameters);

		$.ajax({
			type: "POST",
			url: LogWhatever.CurrentVirtualDirectory + me._dataSourceUrl,
			data: JSON.stringify(data),
			success: callback,
			dataType: "json",
			contentType: "application/json; charset=utf-8"
		});
	});
};

LogWhatever.Controls.Table.prototype._deriveCacheKey = function (page, sort, direction, filter) {
	return JSON.stringify({ page: page, sort: sort, direction: direction, filter: filter });
};

LogWhatever.Controls.Table.create = function(params) {
	return new LogWhatever.Controls.Table(params);
};

(function ($) {
	$.fn.showOrHide = function (flag) {
		this.each(function () {
			if (flag)
				$(this).show();
			else
				$(this).hide();
		});
	};
})(jQuery);