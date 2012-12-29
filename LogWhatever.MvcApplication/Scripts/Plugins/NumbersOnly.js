(function ($) {
	$.fn.numbersOnly = function () {
		return this.each(function () {
			$(this).live("keypress", function (e) {
				var regex = "([0-9]";
				if ($(this).val().indexOf(".") == -1)
					regex += "|\\.";
				if ($(this).val() == "")
					regex += "|-";
				regex += ")";
				return new RegExp(regex).test(String.fromCharCode(e.keyCode));
			});
		});
	};
})(jQuery);