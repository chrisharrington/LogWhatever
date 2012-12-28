using System.Web.Mvc;

namespace LogWhatever.Service.Authentication
{
	public class CustomAuthorizeAttribute : AuthorizeAttribute
	{
		#region Public Methods
		public override void OnAuthorization(AuthorizationContext context)
		{
			if (context.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), false))
				return;

			base.OnAuthorization(context);
		}
		#endregion
	}
}