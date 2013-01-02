using LogWhatever.Common.Models;

namespace LogWhatever.Messages.Commands
{
	public class AddTag : BaseCommand
	{
		#region Properties
		public string Name { get; set; }
		#endregion

		#region Public Methods
		public static AddTag CreateFrom(Tag tag)
		{
			return new AddTag {
				Id = tag.Id,
				Name = tag.Name,
				UpdatedDate = tag.UpdatedDate
			};
		}
		#endregion
	}
}