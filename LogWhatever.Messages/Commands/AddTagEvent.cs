using System;
using LogWhatever.Common.Models;

namespace LogWhatever.Messages.Commands
{
	public class AddTagEvent : BaseCommand
	{
		#region Properties
		public Guid UserId { get; set; }
		public Guid TagId { get; set; }
		public Guid EventId { get; set; }
		public string Name { get; set; }
		public Guid LogId { get; set; }
		public string LogName { get; set; }
		#endregion

		#region Public Methods
		public static AddTagEvent CreateFrom(TagEvent tagEvent)
		{
			return new AddTagEvent {
				EventId = tagEvent.EventId,
				Id = tagEvent.Id,
				Name = tagEvent.Name,
				TagId = tagEvent.TagId,
				UpdatedDate = tagEvent.UpdatedDate,
				UserId = tagEvent.UserId,
				LogId = tagEvent.LogId,
				LogName = tagEvent.LogName
			};
		}
		#endregion
	}
}