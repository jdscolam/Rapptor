using Rapptor.Domain.Request;

namespace Rapptor.Domain
{
	public interface IApiCaller
	{
		TReturn ApiGet<TReturn>(string endpointToCall, string accessToken = null, params RequestParameter[] requestParameters) where TReturn : new();
		TReturn ApiPost<TReturn>(string endpointToCall, string accessToken = null, params RequestParameter[] requestParameters) where TReturn : new();
		TReturn ApiDelete<TReturn>(string endpointToCall, string accessToken = null) where TReturn : new();
		TReturn ApiPost<TBody, TReturn>(string endpointToCall, TBody body = null, string accessToken = null, params RequestParameter[] requestParameters) where TReturn : new() where TBody : class, new();
	}
}