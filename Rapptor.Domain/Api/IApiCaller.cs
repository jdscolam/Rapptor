using Rapptor.Domain.Request;
using Rapptor.Domain.Response;

namespace Rapptor.Domain.Api
{
	public interface IApiCaller
	{
		ResponseEnvelope<TReturn> ApiGet<TReturn>(string endpointToCall, string accessToken, params RequestParameter[] requestParameters) where TReturn : new();
		ResponseEnvelope<TReturn> ApiPost<TReturn>(string endpointToCall, string accessToken, params RequestParameter[] requestParameters) where TReturn : new();
		ResponseEnvelope<TReturn> ApiDelete<TReturn>(string endpointToCall, string accessToken) where TReturn : new();
		ResponseEnvelope<TReturn> ApiPost<TBody, TReturn>(string endpointToCall, string accessToken, TBody body = null, params RequestParameter[] requestParameters) where TReturn : new() where TBody : class, new();
	}
}