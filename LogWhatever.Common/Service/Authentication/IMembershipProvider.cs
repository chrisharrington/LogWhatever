using System.Web.Security;

namespace LogWhatever.Common.Service.Authentication
{
    public interface IMembershipProvider
    {
		#region Public Methods
		MembershipUser CreateUser(string username, string password, string email, string role, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status);
		MembershipUser GetUser(string username, bool userIsOnline);
		bool ChangePassword(MembershipUser membershipUser, string oldPassword, string newPassword);
		bool ValidateUser(string username, string password);
		string ErrorCodeToString(MembershipCreateStatus createStatus);
		void ResetPassword(MembershipUser membershipUser);
		void DeleteUser(string username);
		#endregion
    }
}
