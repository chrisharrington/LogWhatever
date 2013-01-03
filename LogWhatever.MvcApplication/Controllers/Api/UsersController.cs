﻿using System;
using System.Web.Mvc;
using System.Web.Security;
using LogWhatever.Common.Models;
using LogWhatever.Common.Service.Authentication;
using LogWhatever.Messages.Commands;

namespace LogWhatever.MvcApplication.Controllers.Api
{
	public class UsersController : BaseApiController
	{
		#region Properties
		public IMembershipProvider MembershipProvider { get; set; }
		#endregion

		#region Public Methods
		[ActionName("signed-in")]
		public User GetSignedInUser()
		{
			return GetCurrentlySignedInUser();
		}

		[ActionName("sign-in")]
		[AcceptVerbs("GET")]
		public User SignIn(string emailAddress, string password, bool staySignedIn)
		{
			if (string.IsNullOrEmpty(emailAddress))
				throw new ArgumentNullException("emailAddress");
			if (string.IsNullOrEmpty(password))
				throw new ArgumentNullException("password");

			var isUserValidated = MembershipProvider.ValidateUser(emailAddress, password);
			if (!isUserValidated)
				return null;

			FormsAuthentication.SetAuthCookie(emailAddress, staySignedIn);
			return UserRepository.Email(emailAddress);
		}

		[ActionName("register")]
		[AcceptVerbs("POST")]
		public User Register(string name, string email, string password)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException("name");
			if (string.IsNullOrEmpty(email))
				throw new ArgumentNullException("email");
			if (string.IsNullOrEmpty(password))
				throw new ArgumentNullException("password");

			MembershipCreateStatus status;
			MembershipProvider.CreateUser(email, password, email, "User", null, null, true, null, out status);
			if (status != MembershipCreateStatus.Success)
				throw new MembershipCreateUserException(status);

			var user = new User {Name = name, EmailAddress = email};
			Dispatcher.Dispatch(AddUser.CreateFrom(user));
			return user;
		}
		#endregion
	}
}