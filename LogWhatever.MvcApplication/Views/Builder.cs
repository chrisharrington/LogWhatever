using System.Web;

namespace LogWhatever.MvcApplication.Views
{
	public static class Builder
	{
		#region Public Methods
		public static IHtmlString FormText(string label, string id, string defaultValue = "")
		{
			return new HtmlString("<div><span>" + label + "</span><input type=\"text\" value=\"" + defaultValue + "\" id=\"" + id + "\" /></div>");
		}

		public static IHtmlString FormPassword(string label, string id, string defaultValue = "")
		{
			return new HtmlString("<div><span>" + label + "</span><input type=\"password\" value=\"" + defaultValue + "\" id=\"" + id + "\" /></div>");
		}
		#endregion
	}
}