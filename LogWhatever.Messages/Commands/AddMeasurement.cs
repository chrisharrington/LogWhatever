﻿using System;
using LogWhatever.Common.Models;

namespace LogWhatever.Messages.Commands
{
	public class AddMeasurement : BaseCommand
	{
		#region Properties
		public string Name { get; set; }
		public Guid UserId { get; set; }
		public Guid LogId { get; set; }
		public string LogName { get; set; }
		public Guid EventId { get; set; }
		public string Unit { get; set; }
		#endregion

		#region Public Methods
		public static AddMeasurement CreateFrom(Measurement measurement)
		{
			return new AddMeasurement {
				EventId = measurement.EventId,
				Id = measurement.Id,
				LogId = measurement.LogId,
				LogName = measurement.LogName,
				Name = measurement.Name,
				Unit = measurement.Unit,
				UpdatedDate = measurement.UpdatedDate,
				UserId = measurement.UserId
			};
		}
		#endregion
	}
}