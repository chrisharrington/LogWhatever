﻿using LogWhatever.Common.Models;

namespace LogWhatever.Common.Repositories
{
	public interface IUserRepository
	{
		#region Public Methods
		User Email(string emailAddress);
		#endregion
	}
}