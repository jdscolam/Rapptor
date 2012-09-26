using System.Collections.Generic;
using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;
using Rapptor.Api;
using Rapptor.Api.ApiCaller.RestSharp;
using Rapptor.Domain;
using Rapptor.Domain.Api;
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
			var tokenInfo = restSharpApiCaller.ApiGet<TokenInfo>("token", null);

			//Verify
            tokenInfo.Data.ShouldNotBeNull();
			tokenInfo.Data.User.ShouldNotBeNull();
            tokenInfo.Data.User.Description.ShouldNotBeNull();
            tokenInfo.Data.User.Counts.ShouldNotBeNull();

			//Teardown
		}

		[Test]
		public void RestSharpApiCallerCanGetEndpointWithVariable()
		{
			//Setup
			const string userId = "1";
			IApiCaller restSharpApiCaller = new RestSharpApiCaller(ACCESS_TOKEN);

			//Execute
			var user = restSharpApiCaller.ApiGet<User>("users/" + userId, null);

			//Verify
            user.Data.ShouldNotBeNull();
            user.Data.Description.ShouldNotBeNull();
            user.Data.Counts.ShouldNotBeNull();

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
			var postCreated = restSharpApiCaller.ApiPost<Post>("posts/", null, parameters.ToArray());

			//Verify
            postCreated.Data.ShouldNotBeNull();
            postCreated.Data.Id.ShouldNotBeNull();
            postCreated.Data.CreatedAt.ShouldNotBeNull();
            postCreated.Data.Text.ShouldNotBeNull();
            postCreated.Data.Text.ShouldEqual(createPostRequest.Text);
            postCreated.Data.Entities.ShouldNotBeNull();
            postCreated.Data.Entities.Links.ShouldNotBeNull();
            postCreated.Data.Entities.Links[0].ShouldNotBeNull();
            postCreated.Data.ReplyTo.ShouldNotBeNull();
            postCreated.Data.ReplyTo.ShouldEqual(createPostRequest.ReplyTo);
			

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
			var postCreated = restSharpApiCaller.ApiPost<CreatePostRequest, Post>("posts/", null, createPostRequest);
			
			//Verify
            postCreated.Data.ShouldNotBeNull();
            postCreated.Data.Id.ShouldNotBeNull();

            postCreated = restSharpApiCaller.ApiGet<Post>("posts/" + postCreated.Data.Id + "/", null, requestParameters: parameters);

            postCreated.Data.Annotations.ShouldNotBeNull();
            postCreated.Data.Annotations.ShouldHaveCount(1);
            postCreated.Data.Annotations.First().Type.ShouldEqual(annotation.Type);

            var myAnnotationObjectValue = postCreated.Data.Annotations.First().Value as MyAnnotationClass;
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
			var filteredPosts = restSharpApiCaller.ApiGet<List<Post>>("posts/" + postId + "/" + PostsService.REPLIES_ACTION, null, requestParameters.ToArray());

			//Verify
			filteredPosts.ShouldNotBeNull();

            foreach (var reply in filteredPosts.Data)
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
