using FubuTestingSupport;
using NUnit.Framework;
using Rapptor.Api;
using Rapptor.Domain;
using Rapptor.Domain.Response;

namespace Rapptor.Tests.Integration
{
	[TestFixture]
	public class RestSharpApiCallerSpec
	{
		private const string ACCESS_TOKEN = "[INSERT ACCESS TOKEN HERE]";

		[Test]
		public void RestSharpApiCallerCanGetSimpleEndpoint()
		{
			//Setup
			IApiCaller restSharpApiCaller = new RestSharpApiCaller(ACCESS_TOKEN);

			//Execute
			var tokenInfo = restSharpApiCaller.ApiGet<TokenInfoResponse>("token");

			//Verify
			tokenInfo.ShouldNotBeNull();
			tokenInfo.User.ShouldNotBeNull();
			tokenInfo.User.Description.ShouldNotBeNull();
			tokenInfo.User.Counts.ShouldNotBeNull();

			//Teardown
		}

		[Test]
		public void RestSharpApiCallerCanGetEndpointWithVariable()
		{
			//Setup
			const string userId = "1";
			IApiCaller restSharpApiCaller = new RestSharpApiCaller(ACCESS_TOKEN);

			//Execute
			var user = restSharpApiCaller.ApiGet<User>("users/" + userId);

			//Verify
			user.ShouldNotBeNull();
			user.ShouldNotBeNull();
			user.Description.ShouldNotBeNull();
			user.Counts.ShouldNotBeNull();

			//Teardown
		}
	}
}
