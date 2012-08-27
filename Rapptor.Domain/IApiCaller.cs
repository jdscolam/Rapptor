using Rapptor.Domain.Request;

namespace Rapptor.Domain
{
	public interface IApiCaller
	{
		TReturn ApiGet<TReturn>(string endpointToCall, params RequestParameter[] requestParameters) where TReturn : new();
		TReturn ApiPost<TReturn>(string endpointToCall, params RequestParameter[] requestParameters) where TReturn : new();
		TReturn ApiDelete<TReturn>(string endpointToCall) where TReturn : new();
	}
}