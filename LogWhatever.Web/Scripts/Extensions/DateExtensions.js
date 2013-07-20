﻿
Date.prototype.toReadableDateString = function () {
	var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
	return months[this.getMonth()] + " " + this.getDate() + ", " + this.getFullYear();
};

Date.prototype.toLongDateString = function () {
	var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
	var days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
	return days[this.getDay()] + ", " + months[this.getMonth()] + " " + this.getDate() + ", " + this.getFullYear();
};

Date.prototype.toShortDateString = function () {
	return (this.getMonth() + 1) + "/" + this.getDate() + "/" + this.getFullYear();
};

Date.prototype.toDateTimeString = function () {
	return this.toShortDateString() + " " + this.toLocaleTimeString();
};

Date.prototype.toServerReadableString = function () {
	return this.getFullYear() + "-" + (this.getMonth() + 1).toString().padLeft(2, "0") + "-" + this.getDate().toString().padLeft(2, "0") + " " + this.getHours().toString().padLeft(2, "0") + ":" + this.getMinutes().toString().padLeft(2, "0") + ":" + this.getSeconds().toString().padLeft(2, "0");
};

Date.prototype.toShortTimeString = function() {
	var hours = this.getHours();
	var isAfternoon = hours > 11;
	if (hours > 12)
		hours -= 12;
	if (hours == 0)
		hours = 12;
	return hours + ":" + this.getMinutes().toString().padLeft(2, "0") + " " + (isAfternoon ? "PM" : "AM");
};

Date.prototype.clone = function () {
	return new Date(this.getFullYear(), this.getMonth(), this.getDate(), this.getHours(), this.getMinutes(), this.getSeconds(), this.getMilliseconds());
};

Date.getPastMonthRanges = function (count) {
	var dates = new Array();
	var beginning = new Date();
	beginning.setDate(1);
	for (var i = 0; i < count; i++) {
		var end = beginning.clone();
		end.setMonth(end.getMonth() + 1);
		end.setDate(end.getDate() - 1);
		dates.push({ start: beginning.clone(), end: end.clone() });
		beginning.setMonth(beginning.getMonth() - 1);
	}
	return dates;
};

Date.getPastWeekRanges = function (weeks) {
	var dates = new Array();
	var sunday = Date.getBeginningOfWeek();
	for (var i = 0; i < weeks; i++) {
		var end = sunday.clone();
		end.setDate(end.getDate() + 6);
		dates.push({ start: sunday.clone(), end: end.clone() });
		sunday.setDate(sunday.getDate() - 7);
	}
	return dates;
};

Date.getBeginningOfWeek = function (date) {
	date = new Date(date.getFullYear(), date.getMonth(), date.getDate());
	if (!date)
		date = new Date();
	while (date.getDay() != 0)
		date.setDate(date.getDate() - 1);
	return date;
};

Date.getMonthString = function(monthIndex) {
	return ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"][monthIndex];
};

Date.getDayString = function (dayIndex) {
	return ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"][dayIndex];
};

Date.parseUTC = function (string) {
	var localEpoch = new Date(1970, 1, 1).getTime();
	var utcEpoch = Date.UTC(1970, 1, 1);
	var utcOffset = utcEpoch - localEpoch;

	var date = new Date(Date.parse(string));
	date.setTime(date.getTime() - utcOffset);
	return date;
};

Date.parseSerialized = function(string) {
	return $.parseJSON(string, true);
};

Date.prototype.addDays = function (count) {
	var date = this.clone();
	date.setDate(date.getDate() + count);
	return date;
};

function formatDateToShortDateString(date) {
	if (typeof (date) != typeof (Date))
		date = Date.parse(date);
	return date.toString("M/d/yyyy");
}

function formatDateToShortTimeString(date) {
	if (typeof (date) != typeof (Date))
		date = Date.parse(date);
	return new Date(date).toString("h:mm tt");
}