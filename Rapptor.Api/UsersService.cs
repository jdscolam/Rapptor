using System.Collections.Generic;
using Rapptor.Domain;
using Rapptor.Domain.Api;

namespace Rapptor.Api
{
	public class UsersService : IUsersService
	{
		public const string USERS_ENDPOINT = "users/";
		public const string FOLLOW_ACTION = "follow/";
		public const string FOLLOWING_ACTION = "following/";
		public const string FOLLOWERS_ACTION = "followers/";
		public const string MUTE_ACTION = "mute/";
		public const string MUTED_ACTION = "me/muted";

		private readonly IApiCaller _apiCaller;

		public UsersService(IApiCaller apiCaller)
		{
			_apiCaller = apiCaller;
		}

		/// <summary>
		/// Retrieve a user.
		/// </summary>
		/// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
		/// <returns></returns>
		public User RetrieveUser(string userId)
		{
			var userIdCallString = userId + "/";
			var user = _apiCaller.ApiGet<User>(USERS_ENDPOINT + userIdCallString, null);

			return user;
		}

		/// <summary>
		/// Follow a user on behalf of the current AccessToken.
		/// </summary>
		/// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
		/// <returns></returns>
		public User FollowUser(string userId)
		{
			var userIdCallString = userId + "/" + FOLLOW_ACTION;
			var user = _apiCaller.ApiPost<User>(USERS_ENDPOINT + userIdCallString, null);

			return user;
		}

		/// <summary>
		/// Unfollow a user on behalf of the current AccessToken.
		/// </summary>
		/// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
		/// <returns></returns>
		public User UnfollowUser(string userId)
		{
			var userIdCallString = userId + "/" + FOLLOW_ACTION;
			var user = _apiCaller.ApiDelete<User>(USERS_ENDPOINT + userIdCallString, null);

			return user;
		}

		/// <summary>
		/// Get a list of users being followed by a given user.
		/// </summary>
		/// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
		/// <returns></returns>
		public IEnumerable<User> GetUsersBeingFollowed(string userId)
		{
			var userIdCallString = userId + "/" + FOLLOWING_ACTION;
			var users = _apiCaller.ApiGet<List<User>>(USERS_ENDPOINT + userIdCallString, null);

			return users;
		}

		/// <summary>
		/// Get a list of users who follow a given user.
		/// </summary>
		/// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
		/// <returns></returns>
		public IEnumerable<User> GetFollowers(string userId)
		{
			var userIdCallString = userId + "/" + FOLLOWERS_ACTION;
			var users = _apiCaller.ApiGet<List<User>>(USERS_ENDPOINT + userIdCallString, null);

			return users;
		}
		
		/// <summary>
		/// Mute a user on behalf of the current AccessToken.
		/// </summary>
		/// <param name="userId">May be a userId, or Username.</param>
		/// <returns></returns>
		public User MuteUser(string userId)
		{
			var userIdCallString = userId + "/" + MUTE_ACTION;
			var user = _apiCaller.ApiPost<User>(USERS_ENDPOINT + userIdCallString, null);

			return user;
		}

		/// <summary>
		/// Unmute a user on behalf of the current AccessToken.
		/// </summary>
		/// <param name="userId">May be a userId, or Username.</param>
		/// <returns></returns>
		public User UnmuteUser(string userId)
		{
			var userIdCallString = userId + "/" + MUTE_ACTION;
			var user = _apiCaller.ApiDelete<User>(USERS_ENDPOINT + userIdCallString, null);

			return user;
		}


		/// <summary>
		/// Get a list of users muted by the current AccessToken
		/// </summary>
		/// <returns></returns>
		public IEnumerable<User> GetMutedUsers()
		{
			var users = _apiCaller.ApiGet<List<User>>(USERS_ENDPOINT + MUTED_ACTION, null);

			return users;
		}
	}
}