using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LogWhatever.MvcApplication.Controllers.Api
{
	public class TemplatesController : BaseApiController
	{
		#region Properties
		internal virtual string Location
		{
			get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Replace("file://", "").Substring(1)) + "\\..\\Scripts\\Templates"; }
		}
		#endregion

		#region Public Methods
		[System.Web.Http.AcceptVerbs("GET")]
		[System.Web.Http.AllowAnonymous]
		public IEnumerable<Template> All()
		{
			return GetTemplateFileLocations(Location).Select(x => new Template {Name = FormatName(x), Html = ReadFileContents(x)}).ToArray();
		}
		#endregion

		#region Private Methods
		public static ICollection<string> GetTemplateFileLocations(string location, List<string> currentList = null, bool lookInSubdirectories = true)
		{
			if (currentList == null)
				currentList = new List<string>();

			if (lookInSubdirectories)
				foreach (var directory in Directory.GetDirectories(location))
					GetTemplateFileLocations(directory, currentList);

			currentList.AddRange(Directory.GetFiles(location));

			return currentList;
		}

		public static string ReadFileContents(string fileLocation)
		{
			using (var reader = new StreamReader(new FileStream(fileLocation, FileMode.Open, FileAccess.Read)))
			{
				return reader.ReadToEnd();
			}
		}

		public static string FormatName(string location)
		{
			var name = Path.GetFileNameWithoutExtension(location);
			var folder = Path.GetDirectoryName(location).Split('\\').Last();
			return (folder + name).Aggregate("", (first, second) => first + (first.Length > 0 && !Char.IsUpper(first[first.Length - 1]) && Char.IsUpper(second) ? "-" + second : second.ToString())).ToLower();
		}
		#endregion

		#region Template Class
		public class Template
		{
			public string Name { get; set; }
			public string Html { get; set; }
		}
		#endregion
	}
}