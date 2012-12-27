using System;
using LogWhatever.Common.CQRS;

namespace LogWhatever.Messages.Events
{
    public class BaseEvent : IMessage
	{
		#region Properties
		public Guid Id { get; set; }
		public DateTime UpdatedDate { get; set; }
		public Guid EventId { get; set; }
        public Guid IssuingUserId { get; set; }
        public DateTime IssueDate { get; set; }
		#endregion

		#region Constructors
		public BaseEvent()
		{
			EventId = Guid.NewGuid();
			IssueDate = DateTime.Now;
		}
		#endregion
	}
}
