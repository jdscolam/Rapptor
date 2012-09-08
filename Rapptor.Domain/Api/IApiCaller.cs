using Rapptor.Domain.Request;

namespace Rapptor.Domain.Api
{
	public interface IApiCaller
	{
		TReturn ApiGet<TReturn>(string endpointToCall, string accessToken, params RequestParameter[] requestParameters) where TReturn : new();
		TReturn ApiPost<TReturn>(string endpointToCall, string accessToken, params RequestParameter[] requestParameters) where TReturn : new();
		TReturn ApiDelete<TReturn>(string endpointToCall, string accessToken) where TReturn : new();
		TReturn ApiPost<TBody, TReturn>(string endpointToCall, string accessToken, TBody body = null, params RequestParameter[] requestParameters) where TReturn : new() where TBody : class, new();
	}
}