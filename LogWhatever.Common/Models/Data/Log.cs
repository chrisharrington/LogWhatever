using System;
using System.Collections.Generic;

namespace LogWhatever.Common.Models.Data
{
	public class Log : DateModel
	{
		#region Properties
		public User User { get; set; }
		public string Name { get; set; }
		public DateTime Time { get; set; }
		public IEnumerable<Measurement> Measurements { get; set; } 
		public IEnumerable<Tag> Tags { get; set; } 
		#endregion

		#region Constructors
		public Log()
		{
			Tags = new List<Tag>();
			Measurements = new List<Measurement>();
		}
		#endregion 
	}
}