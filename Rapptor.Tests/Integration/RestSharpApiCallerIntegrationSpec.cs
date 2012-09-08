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
	public class RestSharpApiCallerIntegrationSpec
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
			var postCreated = restSharpApiCaller.ApiPost<Post>("posts/", requestParameters:parameters.ToArray());

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
		public void RestSharpApiCallerCanCreatePostWithAnnotations()
		{
			//Setup
			var annotationValue = new MyAnnotationClass
				                      {
					                      Name = "My test parameter annotation"
										  , Value = 23.5M
				                      };
			var annotation = new Annotation
				                 {
					                 Type = "net.raptorapp.test.request.parameter"
									 , Value = annotationValue
				                 };

			var createPostRequest = new CreatePostRequest
			{
				Text = @"@jdscolam this is another #Rapptor #testpost, with links and stuff.  https://github.com/jdscolam/Rapptor and Rapptor NuGet"
				, ReplyTo = "197934"
				, Annotations = new List<Annotation> { annotation }
			};
			var postStreamGeneralParameters = new PostStreamGeneralParameters { IncludeAnnotations = 1 };
			IApiCaller restSharpApiCaller = new RestSharpApiCaller(ACCESS_TOKEN);

			//Execute
			var parameters = PostsService.GetGeneralParameters(postStreamGeneralParameters).ToArray();
			var postCreated = restSharpApiCaller.ApiPost<CreatePostRequest, Post>("posts/", createPostRequest);
			
			//Verify
			postCreated.ShouldNotBeNull();
			postCreated.Id.ShouldNotBeNull();

			postCreated = restSharpApiCaller.ApiGet<Post>("posts/" + postCreated.Id + "/", requestParameters:parameters);
			
			postCreated.Annotations.ShouldNotBeNull();
			postCreated.Annotations.ShouldHaveCount(1);
			postCreated.Annotations.First().Type.ShouldEqual(annotation.Type);

			var myAnnotationObjectValue = postCreated.Annotations.First().Value as MyAnnotationClass;
			myAnnotationObjectValue.ShouldNotBeNull();
			// ReSharper disable PossibleNullReferenceException
			myAnnotationObjectValue.Name.ShouldEqual(annotationValue.Name);
			// ReSharper restore PossibleNullReferenceException
			myAnnotationObjectValue.Value.ShouldEqual(annotationValue.Value);

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
			var filteredPosts = restSharpApiCaller.ApiGet<List<Post>>("posts/" + postId + "/" + PostsService.REPLIES_ACTION, requestParameters:requestParameters.ToArray());

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

	public class MyAnnotationClass
	{
		public string Name { get; set; }
		public decimal Value { get; set; }
	}
}
