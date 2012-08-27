using System;
using System.Collections.Generic;
using Rapptor.Domain;
using Rapptor.Domain.Request;
using RestSharp;

namespace Rapptor.Api.ApiCaller
{
	public class RestSharpApiCaller : IApiCaller
	{
		private const string API_BASE = @"https://alpha-api.app.net/stream/0/";

		private readonly RestClient _apiClient;
		private readonly string _accessToken;
		
		public RestSharpApiCaller(string accessToken)
		{
			_accessToken = accessToken;
			
			_apiClient = new RestClient(API_BASE);
			//_apiClient.AddDefaultHeader("Authorization", "Bearer=" + _accessToken);
		}

		private static void ProcessRequestParameters(IRestRequest request, IEnumerable<RequestParameter> requestParameters)
		{
			foreach (var requestParameter in requestParameters)
			{
				request.AddParameter(requestParameter.Name, requestParameter.Value);
			}
		}

		public TReturn ApiGet<TReturn>(string endpointToCall, params RequestParameter[] requestParameters) where TReturn : new()
		{
			var request = new RestRequest(endpointToCall, Method.GET) {RootElement = "data"};
			request.AddParameter("access_token", _accessToken);

			if (requestParameters != null)
				ProcessRequestParameters(request, requestParameters);

			var response = _apiClient.Execute<TReturn>(request);

			if(response.ErrorException != null)
				throw response.ErrorException;

			if(response.ErrorMessage != null)
				throw new Exception(string.Format("Api Get of type {0} to endpoint {1} failed with message {2}", typeof(TReturn), API_BASE + endpointToCall, response.ErrorMessage));

			return response.Data;
		}

		public TReturn ApiPost<TReturn>(string endpointToCall, params RequestParameter[] requestParameters) where TReturn : new()
		{
			var request = new RestRequest(endpointToCall, Method.POST) { RootElement = "data" };
			request.AddParameter("access_token", _accessToken);

			if(requestParameters != null)
				ProcessRequestParameters(request, requestParameters);
			
			var response = _apiClient.Execute<TReturn>(request);

			if (response.ErrorException != null)
				throw response.ErrorException;

			if (response.ErrorMessage != null)
				throw new Exception(string.Format("Api Post of type {0} to endpoint {1} failed with message {2}", typeof(TReturn), API_BASE + endpointToCall, response.ErrorMessage));

			return response.Data;
		}

		public TReturn ApiDelete<TReturn>(string endpointToCall) where TReturn : new()
		{
			var request = new RestRequest(endpointToCall, Method.POST) { RootElement = "data" };
			request.AddParameter("access_token", _accessToken);

			var response = _apiClient.Execute<TReturn>(request);

			if (response.ErrorException != null)
				throw response.ErrorException;

			if (response.ErrorMessage != null)
				throw new Exception(string.Format("Api Delete of type {0} to endpoint {1} failed with message {2}", typeof(TReturn), API_BASE + endpointToCall, response.ErrorMessage));

			return response.Data;
		}
	}
}