using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FubuTestingSupport;
using NUnit.Framework;
using Rapptor.Api;
using Rapptor.Domain;
using Rapptor.Domain.Api;
using Rapptor.Domain.Response;

namespace Rapptor.Tests.Unit
{
	[TestFixture]
	public class UsersServiceSpec
	{
		[Test]
		public void UsersServiceCanRetrieveUserInfo()
		{
			//Setup
			const string userId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);
            A.CallTo(() => apiCaller.ApiGet<User>(UsersService.USERS_ENDPOINT + userId + "/", null)).Returns(new ResponseEnvelope<User>
                                                                                                                 {
                                                                                                                     Data = new User { Id = "1" }
                                                                                                                 });

			//Execute
			var userRetrieved = usersService.RetrieveUser(userId);

			//Verify
			userRetrieved.Data.ShouldNotBeNull();
            userRetrieved.Data.Id.ShouldEqual(userId);

			//Teardown
		}

		[Test]
		public void UsersServiceCanFollowAUser()
		{
			//Setup
			const string userId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);
            A.CallTo(() => apiCaller.ApiPost<User>(UsersService.USERS_ENDPOINT + userId + "/" + UsersService.FOLLOW_ACTION, null)).Returns(new ResponseEnvelope<User>
                                                                                                                                               {
                                                                                                                                                   Data = new User { Id = "1", YouFollow = true }
                                                                                                                                               });

			//Execute
			var userFollowed = usersService.FollowUser(userId);

			//Verify
            userFollowed.Data.ShouldNotBeNull();
            userFollowed.Data.Id.ShouldEqual(userId);
            userFollowed.Data.YouFollow.ShouldEqual(true);

			//Teardown
		}

		[Test]
		public void UsersServiceCanUnfollowAUser()
		{
			//Setup
			const string userId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);
            A.CallTo(() => apiCaller.ApiDelete<User>(UsersService.USERS_ENDPOINT + userId + "/" + UsersService.FOLLOW_ACTION, null)).Returns(new ResponseEnvelope<User>
                                                                                                                                                 {
                                                                                                                                                     Data = new User { Id = "1", YouFollow = false }
                                                                                                                                                 });

			//Execute
			var userUnFollowed = usersService.UnfollowUser(userId);

			//Verify
            userUnFollowed.Data.ShouldNotBeNull();
            userUnFollowed.Data.Id.ShouldEqual(userId);
            userUnFollowed.Data.YouFollow.ShouldEqual(false);

			//Teardown
		}

		[Test]
		public void UsersServiceCanGetUsersBeingFollowedByAUser()
		{
			//Setup
			const string userId = "me";
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);
            A.CallTo(() => apiCaller.ApiGet<List<User>>(UsersService.USERS_ENDPOINT + userId + "/" + UsersService.FOLLOWING_ACTION, null)).Returns(new ResponseEnvelope<List<User>>
                                                                                                                                                       {
                                                                                                                                                           Data = new List<User> { new User { Id = "1", YouFollow = true } }
                                                                                                                                                       });

			//Execute
			var usersBeingFollowed = usersService.GetUsersBeingFollowed(userId);

			//Verify
            usersBeingFollowed.Data.ShouldNotBeNull();

            foreach (var user in usersBeingFollowed.Data)
			{
				user.YouFollow.ShouldEqual(true);
			}

			//Teardown
		}

		[Test]
		public void UsersServiceCanGetUsersFollowingAUser()
		{
			//Setup
			const string userId = "me";
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);
			A.CallTo(() => apiCaller.ApiGet<List<User>>(UsersService.USERS_ENDPOINT + userId + "/" + UsersService.FOLLOWERS_ACTION, null)).Returns(new ResponseEnvelope<List<User>>
			                                                                                                                                           {
			                                                                                                                                               Data = new List<User> { new User { Id = "1", YouFollow = true } }
			                                                                                                                                           });

			//Execute
			var followers = usersService.GetFollowers(userId);

			//Verify
			followers.ShouldNotBeNull();

            foreach (var user in followers.Data)
			{
				user.YouFollow.ShouldEqual(true);
			}

			//Teardown
		}

		[Test]
		public void UsersServiceCanMuteAUser()
		{
			//Setup
			const string userId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);
            A.CallTo(() => apiCaller.ApiPost<User>(UsersService.USERS_ENDPOINT + userId + "/" + UsersService.MUTE_ACTION, null)).Returns(new ResponseEnvelope<User>
                                                                                                                                             {
                                                                                                                                                 Data = new User { Id = "1", YouMuted = true }
                                                                                                                                             });

			//Execute
			var userMuted = usersService.MuteUser(userId);

			//Verify
            userMuted.Data.ShouldNotBeNull();
            userMuted.Data.Id.ShouldEqual(userId);
            userMuted.Data.YouMuted.ShouldEqual(true);

			//Teardown
		}

		[Test]
		public void UsersServiceCanUnMuteAUser()
		{
			//Setup
			const string userId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);
            A.CallTo(() => apiCaller.ApiDelete<User>(UsersService.USERS_ENDPOINT + userId + "/" + UsersService.MUTE_ACTION, null)).Returns(new ResponseEnvelope<User>
                                                                                                                                               {
                                                                                                                                                   Data = new User { Id = "1", YouMuted = false }
                                                                                                                                               });

			//Execute
			var userUnmuted = usersService.UnmuteUser(userId);

			//Verify
            userUnmuted.Data.ShouldNotBeNull();
            userUnmuted.Data.Id.ShouldEqual(userId);
            userUnmuted.Data.YouMuted.ShouldEqual(false);

			//Teardown
		}

		[Test]
		public void UsersServiceCanListMutedUsers()
		{
			//Setup
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);
            A.CallTo(() => apiCaller.ApiGet<List<User>>(UsersService.USERS_ENDPOINT + UsersService.MUTED_ACTION, null)).Returns(new ResponseEnvelope<List<User>> { Data = new List<User> { new User { Id = "1", YouMuted = true } } });

			//Execute
			var mutedUsers = usersService.GetMutedUsers();

			//Verify
            mutedUsers.Data.ShouldNotBeNull();

            foreach (var user in mutedUsers.Data)
			{
				user.YouMuted.ShouldEqual(true);
			}

			//Teardown
		}

	    [Test]
	    public void UsersServiceCanListUsersWhoHaveStarredAPost()
	    {
	        //Setup
	        const string postId = "1";
	        var starringUser = new User();
	        var apiCaller = A.Fake<IApiCaller>();
	        var usersService = new UsersService(apiCaller);
            A.CallTo(() => apiCaller.ApiGet<List<User>>(PostsService.POSTS_ENDPOINT + postId + "/" + PostsService.STARS_ACTION, null)).Returns(new ResponseEnvelope<List<User>>
                                                                                                                                                   {
                                                                                                                                                       Data = new List<User> { starringUser }
                                                                                                                                                   });

	        //Execute
	        var starringUsers = usersService.ListUsersWhoHaveStarredPost(postId);

	        //Verify
            starringUsers.Data.ShouldNotBeNull();
            starringUsers.Data.ShouldHaveCount(1);
            starringUsers.Data.First().ShouldNotBeNull();
            starringUsers.Data.First().ShouldEqual(starringUser);

	        //Teardown
	    }

	    [Test]
	    public void UsersServiceCanListUsersWhoHaveRePostedAPost()
	    {
            //Setup
            const string postId = "1";
            var repostingUser = new User();
            var apiCaller = A.Fake<IApiCaller>();
            var usersService = new UsersService(apiCaller);
            A.CallTo(() => apiCaller.ApiGet<List<User>>(PostsService.POSTS_ENDPOINT + postId + "/" + PostsService.REPOSTERS_ACTION, null)).Returns(new ResponseEnvelope<List<User>>
                                                                                                                                                       {
                                                                                                                                                           Data = new List<User> { repostingUser }
                                                                                                                                                       });

	        //Execute
	        var repostingUsers = usersService.ListUsersWhoHaveRepostedPost(postId);

            //Verify
            repostingUsers.Data.ShouldNotBeNull();
            repostingUsers.Data.ShouldHaveCount(1);
            repostingUsers.Data.First().ShouldNotBeNull();
            repostingUsers.Data.First().ShouldEqual(repostingUser);

	        //Teardown
	    }
	}
}
