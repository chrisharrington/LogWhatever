namespace LogWhatever.Common.Service.Email
{
	public interface IEmailer
	{
		#region Public Methods
		void Send(string from, string to, string subject, string body);
		#endregion
	}
}