
(function ($) {
	$.fn.fadeInDeferred = function(duration, callback) {
		return this.each(function () {
			var deferred = new $.Deferred();
			$(this).fadeIn(duration, function () {
				deferred.resolve();
				if (callback)
					callback();
			});
			return deferred.promise();
		});
	};

	$.fn.fadeOutDeferred = function(duration, callback) {
		return this.each(function() {
			var deferred = new $.Deferred();
			$(this).fadeOut(duration, function () {
				deferred.resolve();
				if (callback)
					callback();
			});
			return deferred.promise();
		});
	};
	
	$.fn.slideUpDeferred = function (duration, callback) {
		return this.each(function () {
			var deferred = new $.Deferred();
			$(this).slideUp(duration, function () {
				deferred.resolve();
				if (callback)
					callback();
			});
			return deferred.promise();
		});
	};

	$.fn.slideDownDeferred = function (duration, callback) {
		return this.each(function () {
			var deferred = new $.Deferred();
			$(this).slideDown(duration, function () {
				deferred.resolve();
				if (callback)
					callback();
			});
			return deferred.promise();
		});
	};
})(jQuery);