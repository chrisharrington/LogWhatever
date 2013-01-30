using System;
using System.Web.Security;
using LogWhatever.Common.Configuration;
using LogWhatever.Common.Service.Authentication;
using LogWhatever.Common.Service.Email;

namespace LogWhatever.Service.Authentication
{
    public class AspMembership : IMembershipProvider
    {
		#region Properties
		public IEmailer MailSender { get; set; }
		public IConfigurationProvider ConfigurationProvider { get; set; } 
		#endregion

		#region Public Methods
		public MembershipUser CreateUser(string username, string password, string email, string role, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			var user = Membership.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, out status);
			AddUserToRole(username, status, role);
			return user;
		}

		public MembershipUser GetUser(string username, bool userIsOnline)
		{
			return Membership.GetUser(username, userIsOnline);
		}

		public virtual bool ChangePassword(MembershipUser membershipUser, string oldPassword, string newPassword)
		{
			return membershipUser.ChangePassword(oldPassword, newPassword);
		}

		public bool ValidateUser(string username, string password)
		{
			return Membership.ValidateUser(username, password);
		}

		public string ErrorCodeToString(MembershipCreateStatus createStatus)
		{
			switch (createStatus)
			{
				case MembershipCreateStatus.DuplicateUserName:
					return "User name already exists. Please enter a different user name.";

				case MembershipCreateStatus.DuplicateEmail:
					return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

				case MembershipCreateStatus.InvalidPassword:
					return "The password provided is invalid. Please enter a valid password value.";

				case MembershipCreateStatus.InvalidEmail:
					return "The e-mail address provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidAnswer:
					return "The password retrieval answer provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidQuestion:
					return "The password retrieval question provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.InvalidUserName:
					return "The user name provided is invalid. Please check the value and try again.";

				case MembershipCreateStatus.ProviderError:
					return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				case MembershipCreateStatus.UserRejected:
					return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

				default:
					return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
			}
		}

		public void ResetPassword(MembershipUser membershipUser)
		{
			var newPassword = membershipUser.ResetPassword();
			var body = string.Format(@"This is a password reset notification from LogWhatever.com. Your password has been reset to <b>{0}</b>. You will need to use this new password when you next log on.", newPassword);
			MailSender.Send(ConfigurationProvider.FromEmailAddress, membershipUser.UserName, "LogWhatever Password Reset", body);
		}

		public void DeleteUser(string username)
		{
			if (string.IsNullOrEmpty(username))
				throw new ArgumentNullException("username");

			Membership.DeleteUser(username);
		}
		#endregion

    	#region Private Methods
		private void AddUserToRole(string username, MembershipCreateStatus status, string role)
		{
			if (status != MembershipCreateStatus.Success)
				return;
			
			Roles.AddUserToRole(username, role);
		}
		#endregion
	}
}