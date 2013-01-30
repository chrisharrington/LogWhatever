namespace LogWhatever.Common.Http
{
	public interface IHttpRequestor
	{
		#region Public Methods
		TResponseType Get<TResponseType>(string url);
		#endregion
	}
}