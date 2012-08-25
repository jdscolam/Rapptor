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
		public void TokenServiceCanRetrieveUserInfo()
		{
			//Setup
			const string userId = "1";
			var apiCaller = A.Fake<IApiCaller>();
			var usersService = new UsersService(apiCaller);

			//Execute
			User user = usersService.RetrieveUser(userId);

			//Verify
			user.ShouldNotBeNull();

			//Teardown
		}
	}
}
