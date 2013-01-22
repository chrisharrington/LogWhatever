using System;
using LogWhatever.Common.Models;

namespace LogWhatever.Messages.Commands
{
	public class AddTag : BaseCommand
	{
		#region Properties
		public Guid UserId { get; set; }
		public Guid EventId { get; set; }
		public string Name { get; set; }
		public Guid LogId { get; set; }
		public string LogName { get; set; }
		public DateTime Date { get; set; }
		#endregion

		#region Public Methods
		public static AddTag CreateFrom(Tag tag)
		{
			return new AddTag {
				EventId = tag.EventId,
				Id = tag.Id,
				Name = tag.Name,
				UpdatedDate = tag.UpdatedDate,
				UserId = tag.UserId,
				LogId = tag.LogId,
				LogName = tag.LogName,
				Date = tag.Date
			};
		}
		#endregion
	}
}