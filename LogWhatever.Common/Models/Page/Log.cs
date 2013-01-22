using System;
using System.Collections.Generic;

namespace LogWhatever.Common.Models.Page
{
	public class Log
	{
		#region Properties
		public DateTime Date { get; set; }
		public string Name { get; set; }
		public IEnumerable<Models.Measurement> Measurements { get; set; }
		public IEnumerable<Tag> Tags { get; set; }
		#endregion
	}
}