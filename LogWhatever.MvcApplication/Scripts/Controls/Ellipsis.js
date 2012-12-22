namespace("LogWhatever.Controls");

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Constructors */

LogWhatever.Controls.Ellipsis = function() {};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Public Methods */

LogWhatever.Controls.Ellipsis.addTitleToShortenedElements = function () {
	//	var foo = $("#foo");
	//	var max_width = foo.width();
	//	foo.children().each(function () {
	//		if ($(this).width() > max_width) {
	//			$(this).attr("title", $(this).text());
	//		}
	//	});

	$(".ellipsis").each(function (i, label) {
		if (label.clientWidth < label.scrollWidth)
			$(label).attr("title", $(label).text());
	});
};