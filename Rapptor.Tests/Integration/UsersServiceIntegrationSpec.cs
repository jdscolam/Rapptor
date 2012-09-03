using System.Linq;
using FubuTestingSupport;
using NUnit.Framework;
using Rapptor.Api;
using Rapptor.Api.ApiCaller;

namespace Rapptor.Tests.Integration
{
	[TestFixture]
	public class UsersServiceIntegrationSpec
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
			var myFollowers = usersService.GetFollowers(userId).ToList();

			//Verify
			myFollowers.ShouldNotBeNull();
			myFollowers.Count().ShouldBeGreaterThan(0);

			//Teardown
		}

		[Test]
		public void UsersServiceCanRetrieveFollowersByUserId()
		{
			//Setup
			const string userId = "1";
			var restSharpApiCaller = new RestSharpApiCaller(ACCESS_TOKEN);
			var usersService = new UsersService(restSharpApiCaller);

			//Execute
			var followers = usersService.GetFollowers(userId).ToList();

			//Verify
			followers.ShouldNotBeNull();
			followers.Count().ShouldBeGreaterThan(0);

			//Teardown
		}
	}
}
