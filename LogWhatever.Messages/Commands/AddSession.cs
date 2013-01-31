using System;
using LogWhatever.Common.Models;

namespace LogWhatever.Messages.Commands
{
	public class AddSession : BaseCommand
	{
		#region Properties
		public Guid UserId { get; set; }
		public string Name { get; set; }
		public string EmailAddress { get; set; }
		#endregion

		#region Public Methods
		public static AddSession CreateFrom(Session session)
		{
			return new AddSession {
				EmailAddress = session.EmailAddress,
				Id = session.Id,
				Name = session.Name,
				UserId = session.UserId,
				UpdatedDate = session.UpdatedDate
			};
		}
		#endregion
	}
}