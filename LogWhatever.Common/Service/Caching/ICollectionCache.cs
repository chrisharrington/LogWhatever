using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;

namespace LogWhatever.Common.Service.Caching
{
	public interface ICollectionCache
	{
		#region Public Methods
		bool ContainsKey<TCachedType>();
		IEnumerable<TCachedType> StoreOrRetrieve<TCachedType>(IEnumerable<BaseModel> models) where TCachedType : BaseModel;
		IEnumerable<TCachedType> StoreOrRetrieve<TCachedType>(BaseModel model) where TCachedType : BaseModel;
		IEnumerable<TCachedType> Retrieve<TCachedType>() where TCachedType : BaseModel;
		void RemoveFromList<TCachedType>(Guid id);
		void AddToList<TCachedType>(TCachedType obj) where TCachedType : BaseModel;
		void AddToList<TCachedType>(IEnumerable<TCachedType> list) where TCachedType : BaseModel;
		void UpdateInList<TCachedType>(TCachedType obj) where TCachedType : BaseModel;
		#endregion
	}
}