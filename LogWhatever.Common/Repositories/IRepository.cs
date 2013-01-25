using System.Collections.Generic;

namespace LogWhatever.Common.Repositories
{
	public interface IRepository<out TModel>
	{
		#region Public Methods
		IEnumerable<TModel> All();
		#endregion
	}
}