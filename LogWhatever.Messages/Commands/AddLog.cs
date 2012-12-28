using System;
using LogWhatever.Common.Models;

namespace LogWhatever.Messages.Commands
{
	public class AddLog : BaseCommand
	{
		#region Properties
		public string Name { get; set; }
		public Guid UserId { get; set; }
		#endregion

		#region Public Methods
		public static object CreateFrom(Log log)
		{
			return new AddLog
			{
				Id = log.Id,
				Name = log.Name,
				UpdatedDate = log.UpdatedDate,
				UserId = log.UserId
			};
		}
		#endregion
	}
}