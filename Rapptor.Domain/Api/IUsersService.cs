using System.Collections.Generic;

namespace Rapptor.Domain.Api
{
	public interface IUsersService
	{
		/// <summary>
		/// Retrieve a user.
		/// </summary>
		/// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
		/// <returns></returns>
		User RetrieveUser(string userId);

		/// <summary>
		/// Follow a user on behalf of the current AccessToken.
		/// </summary>
		/// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
		/// <returns></returns>
		User FollowUser(string userId);

		/// <summary>
		/// Unfollow a user on behalf of the current AccessToken.
		/// </summary>
		/// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
		/// <returns></returns>
		User UnfollowUser(string userId);

		/// <summary>
		/// Get a list of users being followed by a given user.
		/// </summary>
		/// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
		/// <returns></returns>
		IEnumerable<User> GetUsersBeingFollowed(string userId);

		/// <summary>
		/// Get a list of users who follow a given user.
		/// </summary>
		/// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
		/// <returns></returns>
		IEnumerable<User> GetFollowers(string userId);

		/// <summary>
		/// Mute a user on behalf of the current AccessToken.
		/// </summary>
		/// <param name="userId">May be a userId, or Username.</param>
		/// <returns></returns>
		User MuteUser(string userId);

		/// <summary>
		/// Unmute a user on behalf of the current AccessToken.
		/// </summary>
		/// <param name="userId">May be a userId, or Username.</param>
		/// <returns></returns>
		User UnmuteUser(string userId);

		/// <summary>
		/// Get a list of users muted by the current AccessToken
		/// </summary>
		/// <returns></returns>
		IEnumerable<User> GetMutedUsers();
	}
}