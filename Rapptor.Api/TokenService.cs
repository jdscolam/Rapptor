using Rapptor.Domain.Api;
using Rapptor.Domain.Response;

namespace Rapptor.Api
{
	public class TokenService : ITokenService
	{
		private const string TOKEN_ENDPOINT = "token";

		private readonly IApiCaller _apiCaller;
		
		public TokenService(IApiCaller apiCaller)
		{
			_apiCaller = apiCaller;
		}

		public ResponseEnvelope<TokenInfo> RetrieveCurrentTokenInfo()
		{
			var tokenResponse = _apiCaller.ApiGet<TokenInfo>(TOKEN_ENDPOINT, null);

			return tokenResponse;
		}
	}
}