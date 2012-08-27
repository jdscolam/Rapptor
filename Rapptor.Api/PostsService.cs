using System.Collections.Generic;
using System.Linq;
using Rapptor.Domain;
using Rapptor.Domain.Request;

namespace Rapptor.Api
{
	public class PostsService
	{
		public const string POSTS_ENDPOINT = "posts/";
		public const string REPLIES_ACTION = "replies/";
		public const string MENTIONS_ENDPOINT = "mentions/";
		public const string STREAM_ENDPOINT = "stream/";
		public const string GLOBAL_ENDPOINT = "global/";
		public const string TAG_ENDPOINT = "tag/";

		private readonly IApiCaller _apiCaller;

		public PostsService(IApiCaller apiCaller)
		{
			_apiCaller = apiCaller;
		}

		/// <summary>
		/// Parses a CreatePostRequest instance into the proper request parameters for App.net.
		/// </summary>
		/// <param name="createPostRequest"></param>
		/// <returns></returns>
		public static IEnumerable<RequestParameter> GetPostRequestParameters(CreatePostRequest createPostRequest)
		{
			if(createPostRequest == null)
				return null;

			var requestParameters = new List<RequestParameter>();

			if (!string.IsNullOrWhiteSpace(createPostRequest.Text)) requestParameters.Add(new RequestParameter { Name = "text", Value = createPostRequest.Text });
			if (!string.IsNullOrWhiteSpace(createPostRequest.ReplyTo)) requestParameters.Add(new RequestParameter { Name = "reply_to", Value = createPostRequest.ReplyTo });
			if (createPostRequest.Annotations != null) requestParameters.Add(new RequestParameter { Name = "annotations", Value = createPostRequest.Annotations });
			if (createPostRequest.Links != null) requestParameters.Add(new RequestParameter { Name = "links", Value = createPostRequest.Links });

			return requestParameters.Count > 0 ? requestParameters : null;
		}

		/// <summary>
		/// Parses a PostStreamGeneralParameters into the proper request parameters for App.net.
		/// </summary>
		/// <param name="postStreamGeneralParameters"></param>
		/// <returns></returns>
		public static IEnumerable<RequestParameter> GetGeneralParameters(PostStreamGeneralParameters postStreamGeneralParameters)
		{
			if(postStreamGeneralParameters == null)
				return null;

			var generalParameters = new List<RequestParameter>();

			if (!string.IsNullOrWhiteSpace(postStreamGeneralParameters.SinceId)) generalParameters.Add(new RequestParameter { Name = "since_id", Value = postStreamGeneralParameters.SinceId });
			if (!string.IsNullOrWhiteSpace(postStreamGeneralParameters.BeforeId)) generalParameters.Add(new RequestParameter { Name = "before_id", Value = postStreamGeneralParameters.BeforeId });
			if (postStreamGeneralParameters.Count.HasValue) generalParameters.Add(new RequestParameter { Name = "count", Value = postStreamGeneralParameters.Count });
			if (postStreamGeneralParameters.IncludeUser.HasValue) generalParameters.Add(new RequestParameter { Name = "include_user", Value = postStreamGeneralParameters.IncludeUser });
			if (postStreamGeneralParameters.IncludeAnnotations.HasValue) generalParameters.Add(new RequestParameter { Name = "include_annotations", Value = postStreamGeneralParameters.IncludeAnnotations });
			if (postStreamGeneralParameters.IncludeReplies.HasValue) generalParameters.Add(new RequestParameter { Name = "include_replies", Value = postStreamGeneralParameters.IncludeReplies });

			return generalParameters.Count > 0 ? generalParameters : null;
		}

		/// <summary>
		/// Creates a new post as the current AccessToken.
		/// </summary>
		/// <param name="createPostRequest"></param>
		/// <returns></returns>
		public Post CreatePost(CreatePostRequest createPostRequest)
		{
			var postRequestParameters = GetPostRequestParameters(createPostRequest);

			var post = postRequestParameters != null 
				? _apiCaller.ApiPost<Post>(POSTS_ENDPOINT, postRequestParameters.ToArray()) 
				: _apiCaller.ApiPost<Post>(POSTS_ENDPOINT);

			return post;
		}
		
		/// <summary>
		/// Retrieves a post by postId.
		/// </summary>
		/// <param name="postId"></param>
		/// <returns></returns>
		public Post RetrievePost(string postId)
		{
			var postIdCallString = postId + "/";
			var post = _apiCaller.ApiGet<Post>(POSTS_ENDPOINT + postIdCallString);

			return post;
		}

		/// <summary>
		/// Deletes a post by postId as the current AccessToken.
		/// </summary>
		/// <param name="postId"></param>
		/// <returns></returns>
		public Post DeletePost(string postId)
		{
			var postIdCallString = postId + "/";
			var post = _apiCaller.ApiDelete<Post>(POSTS_ENDPOINT + postIdCallString);

			return post;
		}

		/// <summary>
		/// Retrieves a list of posts that have replied to the given postId.
		/// </summary>
		/// <param name="postId"></param>
		/// <param name="postStreamGeneralParameters"> </param>
		/// <returns></returns>
		public IEnumerable<Post> RetrievePostReplies(string postId, PostStreamGeneralParameters postStreamGeneralParameters = null)
		{
			var postIdCallString = postId + "/" + REPLIES_ACTION;

			var generalParameters = GetGeneralParameters(postStreamGeneralParameters);

			var posts = generalParameters != null
				? _apiCaller.ApiGet<List<Post>>(POSTS_ENDPOINT + postIdCallString, generalParameters.ToArray())
				: _apiCaller.ApiGet<List<Post>>(POSTS_ENDPOINT + postIdCallString);

			return posts;
		}

		/// <summary>
		/// Retrieves a list of posts created by a given userId.
		/// </summary>
		/// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
		/// <param name="postStreamGeneralParameters"></param>
		/// <returns></returns>
		public IEnumerable<Post> RetrievePostsCreatedByUser(string userId, PostStreamGeneralParameters postStreamGeneralParameters = null)
		{
			var userIdPostsCallString = userId + "/" + POSTS_ENDPOINT;

			var generalParameters = GetGeneralParameters(postStreamGeneralParameters);

			var posts = generalParameters != null
				? _apiCaller.ApiGet<List<Post>>(UsersService.USERS_ENDPOINT + userIdPostsCallString, generalParameters.ToArray())
				: _apiCaller.ApiGet<List<Post>>(UsersService.USERS_ENDPOINT + userIdPostsCallString);

			return posts;
		}

		/// <summary>
		/// Retrieves a list of posts where the given userId was mentioned.
		/// </summary>
		/// <param name="userId">May be a userId, Username, or "me" for the current user.</param>
		/// <param name="postStreamGeneralParameters"></param>
		/// <returns></returns>
		public IEnumerable<Post> RetrievePostsMentioningUser(string userId, PostStreamGeneralParameters postStreamGeneralParameters = null)
		{
			var userIdMentionssCallString = userId + "/" + MENTIONS_ENDPOINT;

			var generalParameters = GetGeneralParameters(postStreamGeneralParameters);

			var posts = generalParameters != null
				? _apiCaller.ApiGet<List<Post>>(UsersService.USERS_ENDPOINT + userIdMentionssCallString, generalParameters.ToArray())
				: _apiCaller.ApiGet<List<Post>>(UsersService.USERS_ENDPOINT + userIdMentionssCallString);

			return posts;
		}

		/// <summary>
		/// Retrieves the stream for the current AccessToken.
		/// 
		/// NOTE:  Unless changed by the postStreamGeneralParameters, only the latest 20 posts will be returned.
		/// </summary>
		/// <param name="postStreamGeneralParameters"></param>
		/// <returns></returns>
		public IEnumerable<Post> RetrieveCurrentUsersStream(PostStreamGeneralParameters postStreamGeneralParameters = null)
		{
			const string currentUsersStreamCallString = POSTS_ENDPOINT + STREAM_ENDPOINT;

			var generalParameters = GetGeneralParameters(postStreamGeneralParameters);

			var posts = generalParameters != null
				? _apiCaller.ApiGet<List<Post>>(currentUsersStreamCallString, generalParameters.ToArray())
				: _apiCaller.ApiGet<List<Post>>(currentUsersStreamCallString);

			return posts;
		}

		/// <summary>
		/// Retrieves the global stream for the current AccessToken.
		/// 
		/// NOTE:  Unless changed by the postStreamGeneralParameters, only the latest 20 posts will be returned.
		/// </summary>
		/// <param name="postStreamGeneralParameters"></param>
		/// <returns></returns>
		public IEnumerable<Post> RetrieveGlobalStream(PostStreamGeneralParameters postStreamGeneralParameters = null)
		{
			const string globalStreamCallString = POSTS_ENDPOINT + STREAM_ENDPOINT + GLOBAL_ENDPOINT;

			var generalParameters = GetGeneralParameters(postStreamGeneralParameters);

			var posts = generalParameters != null
				? _apiCaller.ApiGet<List<Post>>(globalStreamCallString, generalParameters.ToArray())
				: _apiCaller.ApiGet<List<Post>>(globalStreamCallString);

			return posts;
		}

		/// <summary>
		/// Retrieves posts matching the given hashtag.
		/// 
		/// NOTE:  Unless changed by the postStreamGeneralParameters, only the latest 20 posts will be returned.
		/// </summary>
		/// <param name="hashtag">The hashtag to search without the # character.</param>
		/// <param name="postStreamGeneralParameters"></param>
		/// <returns></returns>
		public IEnumerable<Post> RetrieveTaggedPosts(string hashtag, PostStreamGeneralParameters postStreamGeneralParameters = null)
		{
			var taggedPostsCallString = POSTS_ENDPOINT + TAG_ENDPOINT + hashtag + "/";

			var generalParameters = GetGeneralParameters(postStreamGeneralParameters);

			var posts = generalParameters != null
				? _apiCaller.ApiGet<List<Post>>(taggedPostsCallString, generalParameters.ToArray())
				: _apiCaller.ApiGet<List<Post>>(taggedPostsCallString);

			return posts;
		}
	}
}