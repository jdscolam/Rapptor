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
		public void RestSharpAipCallerCanGetTokenInfo()
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
	}
}
