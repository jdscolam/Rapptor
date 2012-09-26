using System.Collections.Generic;
using Rapptor.Domain.Request;
using Rapptor.Domain.Response;

namespace Rapptor.Domain.Api
{
	public interface IPostsService
	{
	    /// <summary>
	    /// Creates a new post as the current AccessToken.
	    /// </summary>
	    /// <param name="createPostRequest"></param>
	    /// <returns></returns>
	    ResponseEnvelope<Post> CreatePost(CreatePostRequest createPostRequest);

	    /// <summary>
	    /// Retrieves a post by postId.
	    /// </summary>
	    /// <param name="postId"></param>
	    /// <returns></returns>
	    ResponseEnvelope<Post> RetrievePost(string postId);

	    /// <summary>
	    /// Deletes a post by postId as the current AccessToken.
	    /// </summary>
	    /// <param name="postId"></param>
	    /// <returns></returns>
	    ResponseEnvelope<Post> DeletePost(string postId);

	    /// <summary>
	    /// Retrieves a list of posts that have replied to the given postId.
	    /// </summary>
	    /// <param name="postId"></param>
	    /// <param name="postStreamGeneralParameters"> </param>
	    /// <returns></returns>
	    ResponseEnvelope<List<Post>> RetrievePostReplies(string postId, PostStreamGeneralParameters postStreamGeneralParameters = null);

	    /// <summary>
	    /// Retrieves a list of posts created by a given userId.
	    /// </summary>
	    /// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
	    /// <param name="postStreamGeneralParameters"></param>
	    /// <returns></returns>
	    ResponseEnvelope<List<Post>> RetrievePostsCreatedByUser(string userId, PostStreamGeneralParameters postStreamGeneralParameters = null);

	    /// <summary>
	    /// Retrieves a list of posts where the given userId was mentioned.
	    /// </summary>
	    /// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
	    /// <param name="postStreamGeneralParameters"></param>
	    /// <returns></returns>
	    ResponseEnvelope<List<Post>> RetrievePostsMentioningUser(string userId, PostStreamGeneralParameters postStreamGeneralParameters = null);

	    /// <summary>
	    /// Retrieves the stream for the current AccessToken.
	    /// 
	    /// NOTE:  Unless changed by the postStreamGeneralParameters, only the latest 20 posts will be returned.
	    /// </summary>
	    /// <param name="postStreamGeneralParameters"></param>
	    /// <returns></returns>
	    ResponseEnvelope<List<Post>> RetrieveCurrentUsersStream(PostStreamGeneralParameters postStreamGeneralParameters = null);

	    /// <summary>
	    /// Retrieves the global stream for the current AccessToken.
	    /// 
	    /// NOTE:  Unless changed by the postStreamGeneralParameters, only the latest 20 posts will be returned.
	    /// </summary>
	    /// <param name="postStreamGeneralParameters"></param>
	    /// <returns></returns>
	    ResponseEnvelope<List<Post>> RetrieveGlobalStream(PostStreamGeneralParameters postStreamGeneralParameters = null);

	    /// <summary>
	    /// Retrieves posts matching the given hashtag.
	    /// 
	    /// NOTE:  Unless changed by the postStreamGeneralParameters, only the latest 20 posts will be returned.
	    /// </summary>
	    /// <param name="hashtag">The hashtag to search without the # character.</param>
	    /// <param name="postStreamGeneralParameters"></param>
	    /// <returns></returns>
	    ResponseEnvelope<List<Post>> RetrieveTaggedPosts(string hashtag, PostStreamGeneralParameters postStreamGeneralParameters = null);

	    ResponseEnvelope<Post> StarPost(string postId);
	    ResponseEnvelope<Post> UnstarPost(string postId);

	    /// <summary>
	    /// Retrieves a list of posts starred by a given userId.
	    /// </summary>
	    /// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
	    /// <param name="postStreamGeneralParameters"></param>
	    /// <returns></returns>
	    ResponseEnvelope<List<Post>> RetrievePostsStarredByUser(string userId, PostStreamGeneralParameters postStreamGeneralParameters = null);

	    ResponseEnvelope<Post> Repost(string postId);
	    ResponseEnvelope<Post> Unrepost(string postId);
	}
}