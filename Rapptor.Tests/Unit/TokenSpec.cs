using FakeItEasy;
using FubuTestingSupport;
using NUnit.Framework;
using Rapptor.Api;
using Rapptor.Domain;

namespace Rapptor.Tests.Unit
{
	[TestFixture]
	public class TokenSpec
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
