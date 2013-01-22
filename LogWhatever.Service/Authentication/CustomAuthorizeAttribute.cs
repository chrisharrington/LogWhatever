using System.Linq;
using System.Web.Http;

namespace LogWhatever.Service.Authentication
{
	public class CustomAuthorizeAttribute : AuthorizeAttribute
	{
		#region Public Methods
		public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext context)
		{
			if (context.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any())
				return;

			base.OnAuthorization(context);
		}
		#endregion
	}
}