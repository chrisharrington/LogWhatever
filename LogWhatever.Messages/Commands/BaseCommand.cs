using System;
using LogWhatever.Common.CQRS;

namespace LogWhatever.Messages.Commands
{
    public class BaseCommand : IMessage
	{
		#region Properties
		public Guid Id { get; set; }
		public DateTime UpdatedDate { get; set; }
		#endregion

		#region Constructors
		public BaseCommand()
		{
			UpdatedDate = DateTime.Now;
		}
		#endregion
	}
}
