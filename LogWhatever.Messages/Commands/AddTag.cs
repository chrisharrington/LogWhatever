using System;
using LogWhatever.Common.Models;

namespace LogWhatever.Messages.Commands
{
	public class AddTag : BaseCommand
	{
		#region Properties
		public string Name { get; set; }
		public Guid UserId { get; set; }
		public Guid LogId { get; set; }
		public string LogName { get; set; }
		public Guid EventId { get; set; }
		#endregion

		#region Public Methods
		public static AddTag CreateFrom(Tag tag)
		{
			return new AddTag {
				EventId = tag.EventId,
				Id = tag.Id,
				LogId = tag.LogId,
				LogName = tag.LogName,
				Name = tag.Name,
				UpdatedDate = tag.UpdatedDate,
				UserId = tag.UserId
			};
		}
		#endregion
	}
}