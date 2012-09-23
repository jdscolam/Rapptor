using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using FubuTestingSupport;
using NUnit.Framework;
using Rapptor.Api;
using Rapptor.Domain;
using Rapptor.Domain.Api;

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
			A.CallTo(() => apiCaller.ApiGet<User>(UsersService.USERS_ENDPOINT + userId + "/", null)).Returns(new User { Id = "1" });

			//Execute
			var userRetrieved = usersService.RetrieveUser(userId);

			//Verify
			userRetrieved.ShouldNotBeNull();
			userRetrieved.Id.ShouldEqual(userId);

			//Teardown
		}

		[Test]
		public void UsersServiceCanFollowAUser()
		{
			//Setup
			const string userId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);
			A.CallTo(() => apiCaller.ApiPost<User>(UsersService.USERS_ENDPOINT + userId + "/" + UsersService.FOLLOW_ACTION, null)).Returns(new User { Id = "1", YouFollow = true });

			//Execute
			var userFollowed = usersService.FollowUser(userId);

			//Verify
			userFollowed.ShouldNotBeNull();
			userFollowed.Id.ShouldEqual(userId);
			userFollowed.YouFollow.ShouldEqual(true);

			//Teardown
		}

		[Test]
		public void UsersServiceCanUnfollowAUser()
		{
			//Setup
			const string userId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);
			A.CallTo(() => apiCaller.ApiDelete<User>(UsersService.USERS_ENDPOINT + userId + "/" + UsersService.FOLLOW_ACTION, null)).Returns(new User { Id = "1", YouFollow = false });

			//Execute
			var userUnFollowed = usersService.UnfollowUser(userId);

			//Verify
			userUnFollowed.ShouldNotBeNull();
			userUnFollowed.Id.ShouldEqual(userId);
			userUnFollowed.YouFollow.ShouldEqual(false);

			//Teardown
		}

		[Test]
		public void UsersServiceCanGetUsersBeingFollowedByAUser()
		{
			//Setup
			const string userId = "me";
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);
			A.CallTo(() => apiCaller.ApiGet<List<User>>(UsersService.USERS_ENDPOINT + userId + "/" + UsersService.FOLLOWING_ACTION, null)).Returns(new List<User> { new User { Id = "1", YouFollow = true } });

			//Execute
			var usersBeingFollowed = usersService.GetUsersBeingFollowed(userId).ToList();

			//Verify
			usersBeingFollowed.ShouldNotBeNull();

			foreach (var user in usersBeingFollowed)
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
			A.CallTo(() => apiCaller.ApiGet<List<User>>(UsersService.USERS_ENDPOINT + userId + "/" + UsersService.FOLLOWERS_ACTION, null)).Returns(new List<User> { new User { Id = "1", YouFollow = true } });

			//Execute
			var followers = usersService.GetFollowers(userId).ToList();

			//Verify
			followers.ShouldNotBeNull();

			foreach (var user in followers)
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
			A.CallTo(() => apiCaller.ApiPost<User>(UsersService.USERS_ENDPOINT + userId + "/" + UsersService.MUTE_ACTION, null)).Returns(new User { Id = "1", YouMuted = true });

			//Execute
			var userMuted = usersService.MuteUser(userId);

			//Verify
			userMuted.ShouldNotBeNull();
			userMuted.Id.ShouldEqual(userId);
			userMuted.YouMuted.ShouldEqual(true);

			//Teardown
		}

		[Test]
		public void UsersServiceCanUnMuteAUser()
		{
			//Setup
			const string userId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);
			A.CallTo(() => apiCaller.ApiDelete<User>(UsersService.USERS_ENDPOINT + userId + "/" + UsersService.MUTE_ACTION, null)).Returns(new User { Id = "1", YouMuted = false });

			//Execute
			var userUnmuted = usersService.UnmuteUser(userId);

			//Verify
			userUnmuted.ShouldNotBeNull();
			userUnmuted.Id.ShouldEqual(userId);
			userUnmuted.YouMuted.ShouldEqual(false);

			//Teardown
		}

		[Test]
		public void UsersServiceCanListMutedUsers()
		{
			//Setup
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);
			A.CallTo(() => apiCaller.ApiGet<List<User>>(UsersService.USERS_ENDPOINT + UsersService.MUTED_ACTION, null)).Returns(new List<User> { new User { Id = "1", YouMuted = true } });

			//Execute
			var mutedUsers = usersService.GetMutedUsers().ToList();

			//Verify
			mutedUsers.ShouldNotBeNull();

			foreach (var user in mutedUsers)
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
            A.CallTo(() => apiCaller.ApiGet<List<User>>(PostsService.POSTS_ENDPOINT + postId + "/" + PostsService.STARS_ACTION, null)).Returns(new List<User> { starringUser });

	        //Execute
	        var starringUsers = usersService.ListUsersWhoHaveStarredPost(postId).ToList();

	        //Verify
	        starringUsers.ShouldNotBeNull();
	        starringUsers.ShouldHaveCount(1);
	        starringUsers.First().ShouldNotBeNull();
	        starringUsers.First().ShouldEqual(starringUser);

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
            A.CallTo(() => apiCaller.ApiGet<List<User>>(PostsService.POSTS_ENDPOINT + postId + "/" + PostsService.REPOSTERS_ACTION, null)).Returns(new List<User> { repostingUser });

	        //Execute
	        var repostingUsers = usersService.ListUsersWhoHaveRepostedPost(postId).ToList();

            //Verify
            repostingUsers.ShouldNotBeNull();
            repostingUsers.ShouldHaveCount(1);
            repostingUsers.First().ShouldNotBeNull();
	        repostingUsers.First().ShouldEqual(repostingUser);

	        //Teardown
	    }
	}
}
