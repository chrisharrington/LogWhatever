using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Caching;

namespace LogWhatever.Service.Caching
{
	public class MemoryCollectionCache : ICollectionCache
	{
		#region Data Members
		private readonly IDictionary<Type, List<BaseModel>> _dictionary; 
		#endregion

		#region Constructors
		public MemoryCollectionCache()
		{
			_dictionary = new ConcurrentDictionary<Type, List<BaseModel>>();
		}
		#endregion

		#region Public Methods
		public void Store<TCachedType>(TCachedType value, TimeSpan duration = new TimeSpan()) where TCachedType : BaseModel
		{
			lock (_dictionary)
			{
				var type = typeof (TCachedType);
				if (!_dictionary.ContainsKey(type))
					_dictionary[type] = new List<BaseModel>();
				_dictionary[type].Add(value);
			}
		}

		public void Store<TCachedType>(IEnumerable<TCachedType> values, TimeSpan duration = new TimeSpan()) where TCachedType : BaseModel
		{
			lock (_dictionary)
			{
				var type = typeof (TCachedType);
				if (!_dictionary.ContainsKey(type))
					_dictionary[type] = new List<BaseModel>();
				_dictionary[type].AddRange(values);
			}
		}

		public IEnumerable<TCachedType> StoreOrRetrieve<TCachedType>(IEnumerable<BaseModel> models) where TCachedType : BaseModel
		{
			lock (_dictionary)
			{
				var type = typeof (TCachedType);
				List<BaseModel> result;
				if (_dictionary.ContainsKey(type))
					result = _dictionary[type];
				else
				{
					_dictionary[type] = result = new List<BaseModel>();
					result.AddRange(models);
				}
				return result.Cast<TCachedType>();
			}
		}

		public IEnumerable<TCachedType> StoreOrRetrieve<TCachedType>(BaseModel model) where TCachedType : BaseModel
		{
			lock (_dictionary)
			{
				var result = _dictionary.ContainsKey(typeof(TCachedType)) ? _dictionary[typeof(TCachedType)] : new List<BaseModel>();
				result.Add(model);
				return result.Cast<TCachedType>();
			}
		}

		public IEnumerable<TCachedType> Retrieve<TCachedType>() where TCachedType : BaseModel
		{
			List<BaseModel> result;
			_dictionary.TryGetValue(typeof (TCachedType), out result);
			return result == null ? new List<TCachedType>() : result.Cast<TCachedType>();
		}

		public void RemoveFromList<TCachedType>(Guid id)
		{
			lock (_dictionary)
			{
				var type = typeof (TCachedType);
				if (!_dictionary.ContainsKey(type))
					return;
				for (var i = 0; i < _dictionary[type].Count; i++)
					if (_dictionary[type][i].Id == id)
					{
						_dictionary[type].RemoveAt(i);
						break;
					}
			}
		}

		public void AddToList<TCachedType>(TCachedType obj) where TCachedType : BaseModel
		{
			if (obj == null)
				throw new ArgumentNullException("obj");

			lock (_dictionary)
			{
				var type = typeof (TCachedType);
				if (!_dictionary.ContainsKey(type))
					_dictionary[type] = new List<BaseModel>();
				_dictionary[type].Add(obj);
			}
		}

		public void UpdateInList<TCachedType>(TCachedType obj) where TCachedType : BaseModel
		{
			if (obj == null)
				throw new ArgumentNullException("obj");

			lock (_dictionary)
			{
				var type = typeof (TCachedType);
				if (!_dictionary.ContainsKey(type))
					return;
				for (var i = 0; i < _dictionary[type].Count; i++)
					if (_dictionary[type][i].Equals(obj))
					{
						_dictionary[type][i] = obj;
						break;
					}
			}
		}
		#endregion
	}
}