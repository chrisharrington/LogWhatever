using System;

namespace LogWhatever.Common.Models
{
	public class BaseModel : DateModel
	{
		#region Properties
		public Guid Id { get; set; }
		public DateTime UpdatedDate { get; set; }
		#endregion

		#region Constructors
		public BaseModel()
		{
			Id = Guid.NewGuid();
			UpdatedDate = DateTime.Now;
		}
		#endregion
	}
}