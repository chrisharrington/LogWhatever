using System;
using Dapper;
using LogWhatever.Common.CQRS;
using LogWhatever.Common.Extensions;
using LogWhatever.Messages.Events;

namespace LogWhatever.Handlers.Denormalizers
{
	public class SessionDenormalizer : BaseDenormalizer, IHandleMessagesOfType<SessionAdded>, IHandleMessagesOfType<SessionDeleted>
	{
		#region Public Methods
		public void Handle(SessionAdded message)
		{
			if (message == null)
				throw new ArgumentNullException("message");

			using (var connection = OpenConnection())
			{
				connection.Insert("Sessions", message);
			}
		}

		public void Handle(SessionDeleted message)
		{
			if (message == null)
				throw new ArgumentNullException("message");

			using (var connection = OpenConnection())
			{
				connection.Execute("delete from Sessions where @Id = @Id", message);
			}
		}
		#endregion
	}
}