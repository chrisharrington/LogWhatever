(function ($) {
	$.queryString = function (key, location) {
		try {
			if (!location)
				location = window.location.href;
			key = key.toLowerCase();
			var url = location;
			var query = url.substring(url.indexOf("?") + 1);
			var parts = query.split("&");
			for (var i = 0; i < parts.length; i++) {
				var split = parts[i].split("=");
				if (split[0].toLowerCase() == key)
					return split[1];
			}
			return undefined;
		} catch (e) {
			return undefined;
		}
	};
})(jQuery);