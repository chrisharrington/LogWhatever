(function ($) {
	$.fn.enter = function (callback) {
		this.each(function () {
			$(this).live("keyup", function (e) {
				if (e.keyCode == 13)
					callback(e);
			});
		});
	};
})(jQuery);