using FakeItEasy;
using FubuTestingSupport;
using NUnit.Framework;
using Rapptor.Api;
using Rapptor.Domain;
using Rapptor.Domain.Api;

namespace Rapptor.Tests.Unit
{
	[TestFixture]
	public class TokenServiceSpec
	{
		[Test]
		public void TokenServiceCanRetrieveCurrentTokenInfo()
		{
			//Setup
			var apiCaller = A.Fake<IApiCaller>();
			var tokenService = new TokenService(apiCaller);

			//Execute
			var currentTokenInfo = tokenService.RetrieveCurrentTokenInfo();

			//Verify
			currentTokenInfo.ShouldNotBeNull();

			//Teardown
		}
	}
}
