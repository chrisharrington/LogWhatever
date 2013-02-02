using System;
using System.Collections.Generic;
using LogWhatever.Common.Models;
using LogWhatever.Repositories;

namespace LogWhatever.RemoteRepositories
{
	public class RemoteEventRepository : EventRepository
	{
		#region Properties
		public BaseRemoteRepository Repository { get; set; } 
		#endregion

		#region Public Methods
		public override IEnumerable<Event> All(Func<Event, bool> filter = null)
		{
			return Repository.All("events", filter);
		}

		public override void Create(Event @event)
		{
			Repository.HttpRequestor.Post(ConfigurationProvider.DataServiceLocation + "events", @event, Repository.SessionManager.GetCurrent());
			Cache.AddToList(@event);
		}
		#endregion
	}
}