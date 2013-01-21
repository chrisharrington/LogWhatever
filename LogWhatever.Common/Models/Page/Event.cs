﻿using System;
using System.Collections.Generic;

namespace LogWhatever.Common.Models.Page
{
	public class Event
	{
		#region Properties
		public DateTime Date { get; set; }
		public IEnumerable<Measurement> Measurements { get; set; }
		public IEnumerable<TagEvent> Tags { get; set; }
		#endregion 
	}
}