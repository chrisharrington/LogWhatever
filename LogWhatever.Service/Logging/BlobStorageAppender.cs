using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LogWhatever.Common.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using log4net.Appender;
using log4net.Core;

namespace LogWhatever.Service.Logging
{
	public class BlobStorageAppender : AppenderSkeleton
	{
		#region Data Members
		private CloudBlobContainer _container;
		private IDictionary<DateTime, CloudBlockBlob> _blobs;
		private readonly ConcurrentQueue<Tuple<CloudBlockBlob, LoggingEvent>> _events;
		private readonly Thread _writeThread;
		#endregion

		#region Properties
		public virtual IConfigurationProvider ConfigurationProvider { get; set; }
		#endregion

		#region Constructors
		public BlobStorageAppender()
		{
			_events = new ConcurrentQueue<Tuple<CloudBlockBlob, LoggingEvent>>();

			ConfigurationProvider = new WebConfigProvider();

			_writeThread = new Thread(WriteQueuedEvents);
			_writeThread.Start();
		}

		~BlobStorageAppender()
		{
			_writeThread.Abort();
		}
		#endregion

		#region Protected Methods
		protected override void Append(LoggingEvent loggingEvent)
		{
			WriteLoggingEventToBlob(loggingEvent, GetBlobFromContainer(GetStorageContainer()));
		}
		#endregion

		#region Private Methods
		internal virtual CloudBlobContainer GetStorageContainer()
		{
			if (_container != null)
				return _container;

			var account = CloudStorageAccount.Parse(ConfigurationProvider.BlobStorage.ConnectionString);
			var client = account.CreateCloudBlobClient();
			_container = client.GetContainerReference("logs");
			_container.CreateIfNotExists();
			return _container;
		}

		internal virtual CloudBlockBlob GetBlobFromContainer(CloudBlobContainer container)
		{
			if (_blobs == null)
				InitializeBlobs(container);

			var date = DateTime.Now.Date;
			if (_blobs.ContainsKey(date))
				return _blobs[date];

			var blob = container.GetBlockBlobReference(date.ToLongDateString() + ".txt");
			blob.Properties.ContentType = "text/plain";
			_blobs[date] = blob;
			return blob;
		}

		internal virtual void InitializeBlobs(CloudBlobContainer container)
		{
			_blobs = new Dictionary<DateTime, CloudBlockBlob>();
			foreach (var item in container.ListBlobs(useFlatBlobListing: true))
			{
				if (item.GetType() != typeof(CloudBlockBlob))
					continue;

				var blob = (CloudBlockBlob) item;
				_blobs[DateTime.Parse(blob.Name.Replace(".txt", ""))] = blob;
			}
		}

		internal virtual void WriteLoggingEventToBlob(LoggingEvent loggingEvent, CloudBlockBlob blob)
		{
			lock (_events)
			{
				_events.Enqueue(new Tuple<CloudBlockBlob, LoggingEvent>(blob, loggingEvent));
				Monitor.Pulse(_events);
			}
		}

		internal virtual void WriteQueuedEvents()
		{
			var result = WaitForResult();

			var blob = result.Item1;
			GetBlockIds(blob, blockIds => {
				var newId = Convert.ToBase64String(Encoding.Default.GetBytes(blockIds.Count().ToString().PadLeft(10, '0')));
				blockIds.Add(newId);
				blob.PutBlock(newId, new MemoryStream(Encoding.Default.GetBytes(RenderLoggingEvent(result.Item2))), null);
				blob.PutBlockList(blockIds);

				WriteQueuedEvents();
			});
		}

		internal virtual Tuple<CloudBlockBlob, LoggingEvent> WaitForResult()
		{
			lock (_events)
			{
				Tuple<CloudBlockBlob, LoggingEvent> result;
				if (!_events.Any())
					Monitor.Wait(_events);

				_events.TryDequeue(out result);
				return result;
			}
		}

		internal virtual void GetBlockIds(CloudBlockBlob blob, Action<IList<string>> callback)
		{
			new Task(() => callback(GetBlockListNames(blob).ToList())).Start();
		}

		internal virtual IEnumerable<string> GetBlockListNames(CloudBlockBlob blob)
		{
			try
			{
				return blob.DownloadBlockList().Select(x => x.Name);
			}
			catch (Exception)
			{
				return new List<string>();
			}
		}
		#endregion
	}
}