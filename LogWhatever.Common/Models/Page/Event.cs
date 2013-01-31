﻿using System;
using System.Collections.Generic;

namespace LogWhatever.Common.Models.Page
{
	public class Event : BaseModel
	{
		#region Properties
		public DateTime Date { get; set; }
		public IEnumerable<Measurement> Measurements { get; set; }
		public IEnumerable<Tag> Tags { get; set; }
		#endregion 
	}
}