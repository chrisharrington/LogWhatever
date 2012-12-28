using System;
using LogWhatever.Common.CQRS;

namespace LogWhatever.Messages.Events
{
    public class BaseEvent : IMessage
	{
		#region Properties
		public Guid Id { get; set; }
		public DateTime UpdatedDate { get; set; }
		#endregion
	}
}
