using System;
using System.Net;
using System.Net.Mail;
using LogWhatever.Common.Configuration;
using LogWhatever.Common.Service.Email;
using SendGridMail;
using SendGridMail.Transport;

namespace LogWhatever.Service.Email
{
	public class SendGridEmailer : IEmailer
	{
		#region Data Members
		private readonly SMTP _smtp;
		#endregion

		#region Constructors
		public SendGridEmailer(IConfigurationProvider configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException("configuration");

			_smtp = SMTP.GenerateInstance(new NetworkCredential(configuration.SendGridUserName, configuration.SendGridPassword));
		}

		public SendGridEmailer() {}
		#endregion

		#region Public Methods
		public void Send(string from, string to, string subject, string body)
		{
			if (string.IsNullOrEmpty(from))
				throw new ArgumentNullException("from");
			if (string.IsNullOrEmpty(to))
				throw new ArgumentNullException("to");
			if (string.IsNullOrEmpty(subject))
				throw new ArgumentNullException("subject");

			_smtp.Deliver(SendGrid.GenerateInstance(new MailAddress(from), new[] { new MailAddress(to) }, new MailAddress[0], new MailAddress[0], subject, body, ConvertToText(body), SendGridMail.TransportType.SMTP));
		}
		#endregion

		#region Private Methods
		private string ConvertToText(string body)
		{
			return body.Replace("<br", "\n\r");
		}
		#endregion
	}
}