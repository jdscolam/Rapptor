using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FubuTestingSupport;
using NUnit.Framework;
using Rapptor.Api;
using Rapptor.Domain;
using Rapptor.Domain.Request;

namespace Rapptor.Tests.Unit
{
	[TestFixture]
	public class PostsServiceSpec
	{
		[Test]
		public void PostsServiceCanCreateAPost()
		{
			//Setup
			var createdAt = DateTime.Now;
			var createPostRequest = new CreatePostRequest
				                        {
											Text = @"@jdscolam this is a test #post, with links and stuff.  https://github.com/jdscolam/Rapptor and Rapptor NuGet"
											, ReplyTo = "197934"
											, Links = new List<Link> { new Link
												                           {
													                           Pos = 94
																			   , Text = "Rapptor NuGet"
																			   , Url = @"http://www.nuget.org/packages?q=rapptor"
												                           }}
				                        };
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(apiCaller).WithReturnType<Post>().Returns(new Post
					         {
						         Id = "1"
								 , CreatedAt = createdAt
								 , Text = createPostRequest.Text
								 , Entities = new Entities { Links = createPostRequest.Links}
								 , ReplyTo = createPostRequest.ReplyTo
					         });

			//Execute
			var postCreated = postsService.CreatePost(createPostRequest);

			//Verify
			postCreated.ShouldNotBeNull();
			postCreated.Id.ShouldNotBeNull();
			postCreated.CreatedAt.ShouldNotBeNull();
			postCreated.CreatedAt.ShouldEqual(createdAt);
			postCreated.Text.ShouldNotBeNull();
			postCreated.Text.ShouldEqual(createPostRequest.Text);
			postCreated.Entities.ShouldNotBeNull();
			postCreated.Entities.Links.ShouldNotBeNull();
			postCreated.Entities.Links.ShouldContain(createPostRequest.Links.First());
			postCreated.ReplyTo.ShouldNotBeNull();
			postCreated.ReplyTo.ShouldEqual(createPostRequest.ReplyTo);

			//Teardown
		}

		[Test]
		public void PostsServiceCanGetASpecificPost()
		{
			//Setup
			const string postId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(() => apiCaller.ApiGet<Post>(PostsService.POSTS_ENDPOINT + postId + "/")).Returns(new Post { Id = "1" });

			//Execute
			var post = postsService.RetrievePost(postId);

			//Verify
			post.ShouldNotBeNull();
			post.Id.ShouldNotBeNull();
			post.Id.ShouldEqual(postId);

			//Teardown
		}

		[Test]
		public void PostsServiceCanDeleteASpecificPost()
		{
			//Setup
			const string postId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(() => apiCaller.ApiDelete<Post>(PostsService.POSTS_ENDPOINT + postId + "/")).Returns(new Post { Id = "1" });

			//Execute
			var deletedPost = postsService.DeletePost(postId);

			//Verify
			deletedPost.ShouldNotBeNull();
			deletedPost.Id.ShouldNotBeNull();
			deletedPost.Id.ShouldEqual(postId);


			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrieveRepliesToASpecificPost()
		{
			//Setup
			const string postId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(() => apiCaller.ApiGet<List<Post>>(PostsService.POSTS_ENDPOINT + postId + "/" + PostsService.REPLIES_ACTION)).Returns(new List<Post> {new Post { ReplyTo = "1" } });

			//Execute
			var postReplies = postsService.RetrievePostReplies(postId).ToList();

			//Verify
			postReplies.ShouldNotBeNull();

			foreach (var reply in postReplies)
			{
				reply.ReplyTo.ShouldEqual(postId);
			}

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrieveRepliesToASpecificPostFilteredByPostStreamGeneralParameters()
		{
			//Setup
			const string postId = "1";
			var postStreamGeneralParameters = new PostStreamGeneralParameters { SinceId = "2"};
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(apiCaller).WithReturnType<List<Post>>().Returns(new List<Post> { new Post { Id = "3", ThreadId = "1" } });

			//Execute
			var postReplies = postsService.RetrievePostReplies(postId, postStreamGeneralParameters).ToList();

			//Verify
			postReplies.ShouldNotBeNull();

			foreach (var reply in postReplies)
			{
				reply.Id.ShouldBeGreaterThan(postStreamGeneralParameters.SinceId);
				reply.ThreadId.ShouldEqual(postId);	//Here we are testing to make sure we are replying to the thread rather than the specific post.
			}

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrievePostsCreatedByASpecficUser()
		{
			//Setup
			const string userId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(() => apiCaller.ApiGet<List<Post>>(UsersService.USERS_ENDPOINT + userId + "/" + PostsService.POSTS_ENDPOINT)).Returns(new List<Post> { new Post { User = new User {Id = userId}} });

			//Execute
			var posts = postsService.RetrievePostsCreatedByUser(userId).ToList();

			//Verify
			posts.ShouldNotBeNull();

			foreach (var post in posts)
			{
				post.User.ShouldNotBeNull();
				post.User.Id.ShouldNotBeNull();
				post.User.Id.ShouldEqual(userId);
			}

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrievePostsCreatedByASpecificUserFilteredByPostStreamGeneralParameters()
		{
			//Setup
			const string userId = "1";
			var postStreamGeneralParameters = new PostStreamGeneralParameters { BeforeId = "10" };
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(apiCaller).WithReturnType<List<Post>>().Returns(new List<Post> { new Post { Id = "5", User = new User { Id = userId } } });

			//Execute
			var posts = postsService.RetrievePostsCreatedByUser(userId, postStreamGeneralParameters).ToList();

			//Verify
			posts.ShouldNotBeNull();

			foreach (var post in posts)
			{
				post.User.ShouldNotBeNull();
				post.User.Id.ShouldNotBeNull();
				post.User.Id.ShouldEqual(userId);
				int.Parse(post.Id).ShouldBeLessThan(int.Parse(postStreamGeneralParameters.BeforeId));
			}

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrievePostsMentioningASpecficUser()
		{
			//Setup
			const string userId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(() => apiCaller.ApiGet<List<Post>>(UsersService.USERS_ENDPOINT + userId + "/" + PostsService.MENTIONS_ENDPOINT))
				.Returns(new List<Post>
					         {
						         new Post
							         {
								         Entities = new Entities
									                    {
										                    Mentions = new List<Mention>
											                               {
												                               new Mention { Id = "1" }
											                               }
									                    }
							         }
					         });

			//Execute
			var posts = postsService.RetrievePostsMentioningUser(userId).ToList();

			//Verify
			posts.ShouldNotBeNull();

			foreach (var post in posts)
			{
				post.Entities.ShouldNotBeNull();
				post.Entities.Mentions.ShouldNotBeNull();
				post.Entities.Mentions.Count.ShouldBeGreaterThan(0);
				post.Entities.Mentions[0].Id.ShouldNotBeNull();
				post.Entities.Mentions[0].Id.ShouldEqual(userId);
			}

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrievePostsMentioningASpecficUserFilteredByPostStreamGeneralParameters()
		{
			//Setup
			const string userId = "1";
			var postStreamGeneralParameters = new PostStreamGeneralParameters { Count = 1 };
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(apiCaller).WithReturnType<List<Post>>()
				.Returns(new List<Post>
					         {
						         new Post
							         {
								         Entities = new Entities
									                    {
										                    Mentions = new List<Mention>
											                               {
												                               new Mention { Id = "1" }
											                               }
									                    }
							         }
					         });

			//Execute
			var posts = postsService.RetrievePostsMentioningUser(userId, postStreamGeneralParameters).ToList();

			//Verify
			posts.ShouldNotBeNull();
			posts.Count.ShouldEqual(postStreamGeneralParameters.Count);

			foreach (var post in posts)
			{
				post.Entities.ShouldNotBeNull();
				post.Entities.Mentions.ShouldNotBeNull();
				post.Entities.Mentions.Count.ShouldBeGreaterThan(0);
				post.Entities.Mentions[0].Id.ShouldNotBeNull();
				post.Entities.Mentions[0].Id.ShouldEqual(userId);
			}

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrieveCurrentUsersStream()
		{
			//Setup
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(() => apiCaller.ApiGet<List<Post>>(PostsService.POSTS_ENDPOINT + PostsService.STREAM_ENDPOINT)).Returns(new List<Post> { new Post() });

			//Execute
			var posts = postsService.RetrieveCurrentUsersStream().ToList();

			//Verify
			posts.ShouldNotBeNull();
			posts.ShouldHaveCount(1);

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrieveCurrentUsersStreamFilteredByPostStreamGeneralParameters()
		{
			//Setup
			var postStreamGeneralParameters = new PostStreamGeneralParameters { IncludeUser = true };
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(apiCaller).WithReturnType<List<Post>>().Returns(new List<Post> { new Post { User = new User() } });

			//Execute
			var posts = postsService.RetrieveCurrentUsersStream(postStreamGeneralParameters).ToList();

			//Verify
			posts.ShouldNotBeNull();
			posts.ShouldHaveCount(1);
			posts[0].User.ShouldNotBeNull();

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrieveGlobalStream()
		{
			//Setup
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(() => apiCaller.ApiGet<List<Post>>(PostsService.POSTS_ENDPOINT + PostsService.STREAM_ENDPOINT + PostsService.GLOBAL_ENDPOINT)).Returns(new List<Post> { new Post() });

			//Execute
			var posts = postsService.RetrieveGlobalStream().ToList();

			//Verify
			posts.ShouldNotBeNull();
			posts.ShouldHaveCount(1);

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrieveGlobalStreamFilteredByPostStreamGeneralParameters()
		{
			//Setup
			var postStreamGeneralParameters = new PostStreamGeneralParameters { IncludeUser = true };
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(apiCaller).WithReturnType<List<Post>>().Returns(new List<Post> { new Post { User = new User() } });

			//Execute
			var posts = postsService.RetrieveGlobalStream(postStreamGeneralParameters).ToList();

			//Verify
			posts.ShouldNotBeNull();
			posts.ShouldHaveCount(1);
			posts[0].User.ShouldNotBeNull();

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrievePostsWithAGivenHashtag()
		{
			//Setup
			var hashtag = "Test";
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(() => apiCaller.ApiGet<List<Post>>(PostsService.POSTS_ENDPOINT + PostsService.TAG_ENDPOINT + hashtag + "/"))
				.Returns(new List<Post>
					         {
						         new Post
							         {
								         Text = "#" + hashtag
										 , Entities = new Entities
									                    {
										                    Hashtags = new List<Hashtag>
											                               {
												                               new Hashtag
													                               {
														                               Name = hashtag
																					   , Pos = 0
													                               }
											                               }
									                    }
							         }
					         });

			//Execute
			var posts = postsService.RetrieveTaggedPosts(hashtag).ToList();

			//Verify
			posts.ShouldNotBeNull();

			foreach (var post in posts)
			{
				post.Text.ShouldContain(hashtag);
				post.Entities.ShouldNotBeNull();
				post.Entities.Hashtags.ShouldNotBeNull();
				post.Entities.Hashtags[0].Name.ShouldEqual(hashtag);
			}

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrievePostsWithAGivenHashtagFilteredByPostStreamGeneralParameters()
		{
			//Setup
			const string hashtag = "Test";
			var postStreamGeneralParameters = new PostStreamGeneralParameters { IncludeUser = true };
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(() => apiCaller.ApiGet<List<Post>>(PostsService.POSTS_ENDPOINT + PostsService.TAG_ENDPOINT + hashtag + "/"))
				.Returns(new List<Post>
					         {
						         new Post
							         {
								         Text = "#" + hashtag
										 , Entities = new Entities
									                    {
										                    Hashtags = new List<Hashtag>
											                               {
												                               new Hashtag
													                               {
														                               Name = hashtag
																					   , Pos = 0
													                               }
											                               }
									                    }
											, User = new User()
							         }
					         });

			//Execute
			var posts = postsService.RetrieveTaggedPosts(hashtag, postStreamGeneralParameters).ToList();

			//Verify
			posts.ShouldNotBeNull();

			foreach (var post in posts)
			{
				post.Text.ShouldContain(hashtag);
				post.Entities.ShouldNotBeNull();
				post.Entities.Hashtags.ShouldNotBeNull();
				post.Entities.Hashtags[0].Name.ShouldEqual(hashtag);
				post.User.ShouldNotBeNull();
			}

			//Teardown
		}
	}
}