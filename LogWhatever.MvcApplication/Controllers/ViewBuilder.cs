using System.Text;
using System.Web;
using LogWhatever.Common.Models;
using LogWhatever.MvcApplication.Controllers.Templates;

namespace LogWhatever.MvcApplication.Controllers
{
	public class ViewBuilder
	{
		#region Public Methods
		public static IHtmlString BuildForLog(PagesController.LogModel log)
		{
			var builder = new StringBuilder();
			builder.AppendLine("<div class=\"tile log\">");
			builder.AppendLine("<div>");
			builder.AppendLine("<h3>" + log.Name + "</h3>");
			builder.AppendLine("<span>" + log.Date + "</span>");
			//builder.AppendLine("<div class=\"measurements\">");
			//foreach (var measurement in log.Measurements)
			//	builder.AppendLine("<div><span class=\"name ellipsis\">" + measurement.Name + "</span><span class=\"value ellipsis\">" + measurement.Quantity.ToString("0.00") + "</span></div>");
			//builder.AppendLine("</div>");
			builder.AppendLine("<div class=\"tags\">");
			foreach (var measurement in log.Measurements)
				builder.Append("<div class=\"floating-tag\">" + measurement.Name + " / " + measurement.Quantity.ToString("0.00") + "</div>");
			foreach (var tag in log.Tags)
				builder.Append("<div class=\"floating-tag\">" + tag.Name + "</div>");
			builder.AppendLine("</div>");
			builder.AppendLine("</div>");
			builder.AppendLine("</div>");
			return new HtmlString(builder.ToString());
		}
		#endregion
	}
}