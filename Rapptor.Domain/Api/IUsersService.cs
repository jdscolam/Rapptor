using System.Collections.Generic;
using Rapptor.Domain.Response;

namespace Rapptor.Domain.Api
{
	public interface IUsersService
	{
	    /// <summary>
	    /// Retrieve a user.
	    /// </summary>
	    /// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
	    /// <returns></returns>
	    ResponseEnvelope<User> RetrieveUser(string userId);

	    /// <summary>
	    /// Follow a user on behalf of the current AccessToken.
	    /// </summary>
	    /// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
	    /// <returns></returns>
	    ResponseEnvelope<User> FollowUser(string userId);

	    /// <summary>
	    /// Unfollow a user on behalf of the current AccessToken.
	    /// </summary>
	    /// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
	    /// <returns></returns>
	    ResponseEnvelope<User> UnfollowUser(string userId);

	    /// <summary>
	    /// Get a list of users being followed by a given user.
	    /// </summary>
	    /// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
	    /// <returns></returns>
	    ResponseEnvelope<List<User>> GetUsersBeingFollowed(string userId);

	    /// <summary>
	    /// Get a list of users who follow a given user.
	    /// </summary>
	    /// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
	    /// <returns></returns>
	    ResponseEnvelope<List<User>> GetFollowers(string userId);

	    /// <summary>
	    /// Mute a user on behalf of the current AccessToken.
	    /// </summary>
	    /// <param name="userId">May be a userId, or Username.</param>
	    /// <returns></returns>
	    ResponseEnvelope<User> MuteUser(string userId);

	    /// <summary>
	    /// Unmute a user on behalf of the current AccessToken.
	    /// </summary>
	    /// <param name="userId">May be a userId, or Username.</param>
	    /// <returns></returns>
	    ResponseEnvelope<User> UnmuteUser(string userId);

	    /// <summary>
	    /// Get a list of users muted by the current AccessToken
	    /// </summary>
	    /// <returns></returns>
	    ResponseEnvelope<List<User>> GetMutedUsers();

	    ResponseEnvelope<List<User>> ListUsersWhoHaveStarredPost(string postId);
	    ResponseEnvelope<List<User>> ListUsersWhoHaveRepostedPost(string postId);
	}
}