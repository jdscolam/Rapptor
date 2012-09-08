using Rapptor.Domain.Response;

namespace Rapptor.Domain.Api
{
	public interface ITokenService
	{
		TokenInfoResponse RetrieveCurrentTokenInfo();
	}
}