namespace("LogWhatever.Controls");

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Constructors */

LogWhatever.Controls.Ellipsis = function() {};

//--------------------------------------------------------------------------------------------------------------------------------------------
/* Public Methods */

LogWhatever.Controls.Ellipsis.addTitleToShortenedElements = function () {
	$(".ellipsis").each(function (i, label) {
		if (label.clientWidth < label.scrollWidth)
			$(label).attr("title", $(label).text());
	});
};