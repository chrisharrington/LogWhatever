(function ($) {
	$.fn.numbersOnly = function () {
		return this.each(function () {
			$(this).live("keypress", function (e) {
				return /([0-9]|\.)/.test(String.fromCharCode(e.keyCode));
			});
		});
	};
})(jQuery);