using Rapptor.Domain;
using Rapptor.Domain.Response;

namespace Rapptor.Api
{
	public class TokenService
	{
		private const string TOKEN_ENDPOINT = "token";

		private readonly IApiCaller _apiCaller;
		
		public TokenService(IApiCaller apiCaller)
		{
			_apiCaller = apiCaller;
		}

		public TokenInfoResponse RetrieveCurrentTokenInfo()
		{
			var tokenResponse = _apiCaller.ApiGet<TokenInfoResponse>(TOKEN_ENDPOINT);

			return tokenResponse;
		}
	}
}