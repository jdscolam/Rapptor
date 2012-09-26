using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FubuTestingSupport;
using NUnit.Framework;
using Rapptor.Api;
using Rapptor.Domain;
using Rapptor.Domain.Api;
using Rapptor.Domain.Request;
using Rapptor.Domain.Response;

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
				                        };
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(apiCaller).WithReturnType<ResponseEnvelope<Post>>().Returns(new ResponseEnvelope<Post> 
            { 
                Data = new Post
					         {
						         Id = "1"
								 , CreatedAt = createdAt
								 , Text = createPostRequest.Text
								 , ReplyTo = createPostRequest.ReplyTo
					         }
            });

			//Execute
			var postCreated = postsService.CreatePost(createPostRequest);

			//Verify
            postCreated.Data.ShouldNotBeNull();
            postCreated.Data.Id.ShouldNotBeNull();
            postCreated.Data.CreatedAt.ShouldNotBeNull();
            postCreated.Data.CreatedAt.ShouldEqual(createdAt);
            postCreated.Data.Text.ShouldNotBeNull();
            postCreated.Data.Text.ShouldEqual(createPostRequest.Text);
            postCreated.Data.ReplyTo.ShouldNotBeNull();
            postCreated.Data.ReplyTo.ShouldEqual(createPostRequest.ReplyTo);

			//Teardown
		}

		[Test]
		public void PostsServiceCanGetASpecificPost()
		{
			//Setup
			const string postId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(() => apiCaller.ApiGet<Post>(PostsService.POSTS_ENDPOINT + postId + "/", null)).Returns(new ResponseEnvelope<Post> { Data = new Post { Id = "1" }});

			//Execute
			var post = postsService.RetrievePost(postId);

			//Verify
            post.Data.ShouldNotBeNull();
            post.Data.Id.ShouldNotBeNull();
            post.Data.Id.ShouldEqual(postId);

			//Teardown
		}

		[Test]
		public void PostsServiceCanDeleteASpecificPost()
		{
			//Setup
			const string postId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
            A.CallTo(() => apiCaller.ApiDelete<Post>(PostsService.POSTS_ENDPOINT + postId + "/", null)).Returns(new ResponseEnvelope<Post> { Data = new Post { Id = "1" } });

			//Execute
			var deletedPost = postsService.DeletePost(postId);

			//Verify
            deletedPost.Data.ShouldNotBeNull();
            deletedPost.Data.Id.ShouldNotBeNull();
            deletedPost.Data.Id.ShouldEqual(postId);


			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrieveRepliesToASpecificPost()
		{
			//Setup
			const string postId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
            A.CallTo(() => apiCaller.ApiGet<List<Post>>(PostsService.POSTS_ENDPOINT + postId + "/" + PostsService.REPLIES_ACTION, null)).Returns(new ResponseEnvelope<List<Post>>
                                                                                                                                                     {
                                                                                                                                                         Data = new List<Post>
                                                                                                                                                                    {
                                                                                                                                                                        new Post { ReplyTo = "1" }
                                                                                                                                                                    }
                                                                                                                                                     });

			//Execute
			var postReplies = postsService.RetrievePostReplies(postId);

			//Verify
			postReplies.ShouldNotBeNull();

            foreach (var reply in postReplies.Data)
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
            A.CallTo(apiCaller).WithReturnType<ResponseEnvelope<List<Post>>>().Returns(new ResponseEnvelope<List<Post>>
                                                                                           {
                                                                                               Data = new List<Post>
                                                                                                          {
                                                                                                              new Post { Id = "3", ThreadId = "1" }
                                                                                                          }
                                                                                           });

			//Execute
			var postReplies = postsService.RetrievePostReplies(postId, postStreamGeneralParameters);

			//Verify
			postReplies.ShouldNotBeNull();

            foreach (var reply in postReplies.Data)
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
            A.CallTo(() => apiCaller.ApiGet<List<Post>>(UsersService.USERS_ENDPOINT + userId + "/" + PostsService.POSTS_ENDPOINT, null)).Returns(new ResponseEnvelope<List<Post>>
                                                                                                                                                     {
                                                                                                                                                         Data = new List<Post>
                                                                                                                                                                    {
                                                                                                                                                                        new Post
                                                                                                                                                                            {
                                                                                                                                                                                User = new User { Id = userId }
                                                                                                                                                                            }
                                                                                                                                                                    }
                                                                                                                                                     });

			//Execute
			var posts = postsService.RetrievePostsCreatedByUser(userId);

			//Verify
			posts.ShouldNotBeNull();

            foreach (var post in posts.Data)
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
            A.CallTo(apiCaller).WithReturnType<ResponseEnvelope<List<Post>>>().Returns(new ResponseEnvelope<List<Post>>
                                                                                           {
                                                                                               Data = new List<Post> { new Post { Id = "5", User = new User { Id = userId } } }
                                                                                           });

			//Execute
			var posts = postsService.RetrievePostsCreatedByUser(userId, postStreamGeneralParameters);

			//Verify
			posts.ShouldNotBeNull();

            foreach (var post in posts.Data)
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
            A.CallTo(() => apiCaller.ApiGet<List<Post>>(UsersService.USERS_ENDPOINT + userId + "/" + PostsService.MENTIONS_ENDPOINT, null))
                .Returns(new ResponseEnvelope<List<Post>>
                {
                    Data = new List<Post>
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
					         }
                });

			//Execute
			var posts = postsService.RetrievePostsMentioningUser(userId);

			//Verify
			posts.ShouldNotBeNull();

            foreach (var post in posts.Data)
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
            A.CallTo(apiCaller).WithReturnType<ResponseEnvelope<List<Post>>>()
                .Returns(new ResponseEnvelope<List<Post>>
                {
                    Data = new List<Post>
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
					         }
                });

			//Execute
			var posts = postsService.RetrievePostsMentioningUser(userId, postStreamGeneralParameters);

			//Verify
            posts.Data.ShouldNotBeNull();
            posts.Data.Count.ShouldEqual(postStreamGeneralParameters.Count);

            foreach (var post in posts.Data)
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
            A.CallTo(() => apiCaller.ApiGet<List<Post>>(PostsService.POSTS_ENDPOINT + PostsService.STREAM_ENDPOINT, null)).Returns(new ResponseEnvelope<List<Post>>
                                                                                                                                       {
                                                                                                                                           Data = new List<Post> { new Post() }
                                                                                                                                       });

			//Execute
			var posts = postsService.RetrieveCurrentUsersStream();

			//Verify
            posts.Data.ShouldNotBeNull();
            posts.Data.ShouldHaveCount(1);

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrieveCurrentUsersStreamFilteredByPostStreamGeneralParameters()
		{
			//Setup
			var postStreamGeneralParameters = new PostStreamGeneralParameters { IncludeUser = 1 };
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
			A.CallTo(apiCaller).WithReturnType<ResponseEnvelope<List<Post>>>().Returns(new ResponseEnvelope<List<Post>>
			                                                                               {
			                                                                                   Data = new List<Post> { new Post { User = new User() } }
			                                                                               });

			//Execute
			var posts = postsService.RetrieveCurrentUsersStream(postStreamGeneralParameters);

			//Verify
            posts.Data.ShouldNotBeNull();
            posts.Data.ShouldHaveCount(1);
            posts.Data[0].User.ShouldNotBeNull();

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrieveGlobalStream()
		{
			//Setup
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
            A.CallTo(() => apiCaller.ApiGet<List<Post>>(PostsService.POSTS_ENDPOINT + PostsService.STREAM_ENDPOINT + PostsService.GLOBAL_ENDPOINT, null)).Returns(new ResponseEnvelope<List<Post>>
                                                                                                                                                                      {
                                                                                                                                                                          Data = new List<Post> { new Post() }
                                                                                                                                                                      });

			//Execute
			var posts = postsService.RetrieveGlobalStream();

			//Verify
            posts.Data.ShouldNotBeNull();
            posts.Data.ShouldHaveCount(1);

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrieveGlobalStreamFilteredByPostStreamGeneralParameters()
		{
			//Setup
			var postStreamGeneralParameters = new PostStreamGeneralParameters { IncludeUser = 1 };
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
            A.CallTo(apiCaller).WithReturnType<ResponseEnvelope<List<Post>>>().Returns(new ResponseEnvelope<List<Post>>
                                                                                           {
                                                                                               Data = new List<Post> { new Post { User = new User() } }
                                                                                           });

			//Execute
			var posts = postsService.RetrieveGlobalStream(postStreamGeneralParameters);

			//Verify
            posts.Data.ShouldNotBeNull();
            posts.Data.ShouldHaveCount(1);
            posts.Data[0].User.ShouldNotBeNull();

			//Teardown
		}

		[Test]
		public void PostsServiceCanRetrievePostsWithAGivenHashtag()
		{
			//Setup
			const string hashtag = "Test";
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
            A.CallTo(() => apiCaller.ApiGet<List<Post>>(PostsService.POSTS_ENDPOINT + PostsService.TAG_ENDPOINT + hashtag + "/", null))
                .Returns(new ResponseEnvelope<List<Post>>
                {
                    Data = new List<Post>
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
					         }
                });

			//Execute
			var posts = postsService.RetrieveTaggedPosts(hashtag);

			//Verify
			posts.ShouldNotBeNull();

            foreach (var post in posts.Data)
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
			var postStreamGeneralParameters = new PostStreamGeneralParameters { IncludeUser = 1 };
			var apiCaller = A.Fake<IApiCaller>();
			var postsService = new PostsService(apiCaller);
            A.CallTo(apiCaller).WithReturnType<ResponseEnvelope<List<Post>>>()
                .Returns(new ResponseEnvelope<List<Post>>
                {
                    Data = new List<Post>
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
					         }
                });

			//Execute
			var posts = postsService.RetrieveTaggedPosts(hashtag, postStreamGeneralParameters);

			//Verify
			posts.ShouldNotBeNull();

            foreach (var post in posts.Data)
			{
				post.Text.ShouldContain(hashtag);
				post.Entities.ShouldNotBeNull();
				post.Entities.Hashtags.ShouldNotBeNull();
				post.Entities.Hashtags[0].Name.ShouldEqual(hashtag);
				post.User.ShouldNotBeNull();
			}

			//Teardown
		}

	    [Test]
	    public void PostsServiceCanStarAPost()
	    {
	        //Setup
	        const string postId = "1";
	        var apiCaller = A.Fake<IApiCaller>();
	        var postsService = new PostsService(apiCaller);
            A.CallTo(() => apiCaller.ApiPost<Post>(PostsService.POSTS_ENDPOINT + postId + "/" + PostsService.STARS_ACTION, null)).Returns(new ResponseEnvelope<Post>
                                                                                                                                              {
                                                                                                                                                  Data = new Post { YouStarred = true }
                                                                                                                                              });

	        //Execute
	        var starredPost = postsService.StarPost(postId);

	        //Verify
            starredPost.Data.ShouldNotBeNull();
            starredPost.Data.YouStarred.HasValue.ShouldBeTrue();
            // ReSharper disable PossibleInvalidOperationException
            starredPost.Data.YouStarred.Value.ShouldBeTrue();
            // ReSharper restore PossibleInvalidOperationException

	        //Teardown
	    }

        [Test]
        public void PostsServiceCanUnstarAPost()
        {
            //Setup
            const string postId = "1";
            var apiCaller = A.Fake<IApiCaller>();
            var postsService = new PostsService(apiCaller);
            A.CallTo(() => apiCaller.ApiDelete<Post>(PostsService.POSTS_ENDPOINT + postId + "/" + PostsService.STARS_ACTION, null)).Returns(new ResponseEnvelope<Post>
                                                                                                                                                {
                                                                                                                                                    Data = new Post { YouStarred = false }
                                                                                                                                                });

            //Execute
            var unstarredPost = postsService.UnstarPost(postId);

            //Verify
            unstarredPost.Data.ShouldNotBeNull();
            unstarredPost.Data.YouStarred.HasValue.ShouldBeTrue();
            // ReSharper disable PossibleInvalidOperationException
            unstarredPost.Data.YouStarred.Value.ShouldBeFalse();
            // ReSharper restore PossibleInvalidOperationException

            //Teardown
        }

        [Test]
        public void PostsServiceCanRetrievePostsStarredByAUser()
        {
            //Setup
            const string userId = "me";
            var apiCaller = A.Fake<IApiCaller>();
            var postsService = new PostsService(apiCaller);
            A.CallTo(() => apiCaller.ApiGet<List<Post>>(UsersService.USERS_ENDPOINT + userId + "/" + PostsService.STARS_ACTION, null))
                .Returns(new ResponseEnvelope<List<Post>>
                             {
                                 Data = new List<Post> { new Post { YouStarred = true } }
                             });

            //Execute
            var posts = postsService.RetrievePostsStarredByUser(userId);

            //Verify
            posts.Data.ShouldNotBeNull();
            posts.Data.ShouldHaveCount(1);
            posts.Data.First().YouStarred.HasValue.ShouldBeTrue();
            // ReSharper disable PossibleInvalidOperationException
            posts.Data.First().YouStarred.Value.ShouldBeTrue();
            // ReSharper restore PossibleInvalidOperationException

            //Teardown
        }

        [Test]
        public void PostsServiceCanRepostAPost()
        {
            //Setup
            const string postId = "1";
            var apiCaller = A.Fake<IApiCaller>();
            var postsService = new PostsService(apiCaller);
            A.CallTo(() => apiCaller.ApiPost<Post>(PostsService.POSTS_ENDPOINT + postId + "/" + PostsService.REPOST_ACTION, null)).Returns(new ResponseEnvelope<Post>
                                                                                                                                               {
                                                                                                                                                   Data = new Post
                                                                                                                                                              {
                                                                                                                                                                  RepostOf = new Post
                                                                                                                                                                                 {
                                                                                                                                                                                     Id = postId
                                                                                                                                                                                     , YouReposted = true
                                                                                                                                                                                 }
                                                                                                                                                              }
                                                                                                                                               });

            //Execute
            var repost = postsService.Repost(postId);

            //Verify
            repost.Data.ShouldNotBeNull();
            repost.Data.RepostOf.ShouldNotBeNull();
            repost.Data.RepostOf.Id.ShouldEqual(postId);
            // ReSharper disable PossibleInvalidOperationException
            repost.Data.RepostOf.YouReposted.Value.ShouldBeTrue();
            // ReSharper restore PossibleInvalidOperationException

            //Teardown
        }

        [Test]
        public void PostsServiceCanUnRepostAPost()
        {
            //Setup
            const string postId = "1";
            var apiCaller = A.Fake<IApiCaller>();
            var postsService = new PostsService(apiCaller);
            A.CallTo(() => apiCaller.ApiDelete<Post>(PostsService.POSTS_ENDPOINT + postId + "/" + PostsService.REPOST_ACTION, null)).Returns(new ResponseEnvelope<Post>
                                                                                                                                                 {
                                                                                                                                                     Data = new Post { Id = postId, YouReposted = false }
                                                                                                                                                 });

            //Execute
            var unrepost = postsService.Unrepost(postId);

            //Verify
            unrepost.Data.ShouldNotBeNull();
            unrepost.Data.RepostOf.ShouldBeNull();
            unrepost.Data.Id.ShouldEqual(postId);
            // ReSharper disable PossibleInvalidOperationException
            unrepost.Data.YouReposted.Value.ShouldBeFalse();
            // ReSharper restore PossibleInvalidOperationException

            //Teardown
        }
	}
}