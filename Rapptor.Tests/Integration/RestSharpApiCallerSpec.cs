using System.Collections.Generic;
using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;
using Rapptor.Api;
using Rapptor.Api.ApiCaller;
using Rapptor.Domain;
using Rapptor.Domain.Request;
using Rapptor.Domain.Response;

namespace Rapptor.Tests.Integration
{
	[TestFixture]
	public class RestSharpApiCallerSpec
	{
		private const string ACCESS_TOKEN = "[INSERT ACCESS TOKEN HERE!]";

		[Test]
		public void RestSharpApiCallerCanGetSimpleEndpoint()
		{
			//Setup
			IApiCaller restSharpApiCaller = new RestSharpApiCaller(ACCESS_TOKEN);

			//Execute
			var tokenInfo = restSharpApiCaller.ApiGet<TokenInfoResponse>("token");

			//Verify
			tokenInfo.ShouldNotBeNull();
			tokenInfo.User.ShouldNotBeNull();
			tokenInfo.User.Description.ShouldNotBeNull();
			tokenInfo.User.Counts.ShouldNotBeNull();

			//Teardown
		}

		[Test]
		public void RestSharpApiCallerCanGetEndpointWithVariable()
		{
			//Setup
			const string userId = "1";
			IApiCaller restSharpApiCaller = new RestSharpApiCaller(ACCESS_TOKEN);

			//Execute
			var user = restSharpApiCaller.ApiGet<User>("users/" + userId);

			//Verify
			user.ShouldNotBeNull();
			user.ShouldNotBeNull();
			user.Description.ShouldNotBeNull();
			user.Counts.ShouldNotBeNull();

			//Teardown
		}

		[Test]
		public void RestSharpApiCallerCanCallEndpointWithSerializedParameters()
		{
			//Setup
			var createPostRequest = new CreatePostRequest
				                        {
											Text = @"@jdscolam this is another #Rapptor #testpost, with links and stuff.  https://github.com/jdscolam/Rapptor and Rapptor NuGet"
											, ReplyTo = "197934"
										};
			IApiCaller restSharpApiCaller = new RestSharpApiCaller(ACCESS_TOKEN);

			//Execute
			var parameters = PostsService.GetPostRequestParameters(createPostRequest);
			var postCreated = restSharpApiCaller.ApiPost<Post>("posts/", parameters.ToArray());

			//Verify
			postCreated.ShouldNotBeNull();
			postCreated.Id.ShouldNotBeNull();
			postCreated.CreatedAt.ShouldNotBeNull();
			postCreated.Text.ShouldNotBeNull();
			postCreated.Text.ShouldEqual(createPostRequest.Text);
			postCreated.Entities.ShouldNotBeNull();
			postCreated.Entities.Links.ShouldNotBeNull();
			postCreated.Entities.Links[0].ShouldNotBeNull();
			postCreated.ReplyTo.ShouldNotBeNull();
			postCreated.ReplyTo.ShouldEqual(createPostRequest.ReplyTo);
			

			//Teardown
		}

		[Test]
		public void RestSharpApiCallerCanGetEndpointWithFilterParameters()
		{
			//Setup
			const string postId = "197934";
			var postStreamGeneralParameters = new PostStreamGeneralParameters { SinceId = "199732" };
			IApiCaller restSharpApiCaller = new RestSharpApiCaller(ACCESS_TOKEN);
			var requestParameters = PostsService.GetGeneralParameters(postStreamGeneralParameters);

			//Execute
			var filteredPosts = restSharpApiCaller.ApiGet<List<Post>>("posts/" + postId +"/" + PostsService.REPLIES_ACTION, requestParameters.ToArray());

			//Verify
			filteredPosts.ShouldNotBeNull();

			foreach (var reply in filteredPosts)
			{
				reply.Id.ShouldBeGreaterThan(postStreamGeneralParameters.SinceId);
				reply.ThreadId.ShouldEqual(postId);
			}

			//Teardown
		}
	}
}
