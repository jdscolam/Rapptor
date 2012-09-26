using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;
using Rapptor.Api;
using Rapptor.Api.ApiCaller.RestSharp;

namespace Rapptor.Tests.Integration
{
	[TestFixture]
	public class RestSharpUsersServiceIntegrationSpec
	{
		private const string ACCESS_TOKEN = "[INSERT ACCESS TOKEN HERE!]";

		[Test]
		public void UsersServiceCanRetrieveFollowersOnMe()
		{
			//Setup
			const string userId = "me";
			var restSharpApiCaller = new RestSharpApiCaller(ACCESS_TOKEN);
			var usersService = new UsersService(restSharpApiCaller);

			//Execute
            var myFollowers = usersService.GetFollowers(userId);

			//Verify
            myFollowers.Data.ShouldNotBeNull();
            myFollowers.Data.Count().ShouldBeGreaterThan(0);

			//Teardown
		}

		[Test]
		public void UsersServiceCanRetrieveFollowersByUserId()
		{
			//Setup
			const string userId = "1";	//NOTE:  This is @dalton's userId so it will take FOREVER to return, but it will return.
			var restSharpApiCaller = new RestSharpApiCaller(ACCESS_TOKEN);
			var usersService = new UsersService(restSharpApiCaller);

			//Execute
			var followers = usersService.GetFollowers(userId);

			//Verify
            followers.Data.ShouldNotBeNull();
            followers.Data.Count().ShouldBeGreaterThan(0);

			//Teardown
		}

		[Test]
		public void UsersServiceCanRetrieveFollowersByUsername()
		{
			//Setup
			const string userId = "@jdscolam";
			var restSharpApiCaller = new RestSharpApiCaller(ACCESS_TOKEN);
			var usersService = new UsersService(restSharpApiCaller);

			//Execute
			var followers = usersService.GetFollowers(userId);

			//Verify
            followers.Data.ShouldNotBeNull();
            followers.Data.Count().ShouldBeGreaterThan(0);

			//Teardown
		}

	    [Test]
	    public void UsersServiceCanListUsersWhoHaveStarredAPost()
	    {
	        //Setup
	        const string postId = "1";
	        var restSharpApiCaller = new RestSharpApiCaller(ACCESS_TOKEN);
	        var usersService = new UsersService(restSharpApiCaller);

	        //Execute
	        var starringUsers = usersService.ListUsersWhoHaveStarredPost(postId);

	        //Verify
            starringUsers.Data.ShouldNotBeNull();
            starringUsers.Data.Count.ShouldBeGreaterThan(0);


	        //Teardown
	    }
	}
}
