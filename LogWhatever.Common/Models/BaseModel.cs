﻿using System;

namespace LogWhatever.Common.Models
{
	public class BaseModel
	{
		#region Properties
		public Guid Id { get; set; }
		public DateTime UpdatedDate { get; set; }
		#endregion
	}
}