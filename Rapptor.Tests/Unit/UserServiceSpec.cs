using FakeItEasy;
using FubuTestingSupport;
using NUnit.Framework;
using Rapptor.Api;
using Rapptor.Domain;

namespace Rapptor.Tests.Unit
{
	[TestFixture]
	public class UserServiceSpec
	{
		[Test]
		public void UsersServiceCanRetrieveUserInfo()
		{
			//Setup
			const string userId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);
			A.CallTo(() => apiCaller.ApiGet<User>(UsersService.USERS_ENDPOINT + userId)).Returns(new User { Id = "1" });

			//Execute
			var userRetrieved = usersService.RetrieveUser(userId);

			//Verify
			userRetrieved.ShouldNotBeNull();
			userRetrieved.Id.ShouldEqual(userId);

			//Teardown
		}
	}
}
