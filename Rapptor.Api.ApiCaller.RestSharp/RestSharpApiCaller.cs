using System;
using System.Collections.Generic;
using Rapptor.Domain.Api;
using Rapptor.Domain.Request;
using Rapptor.Domain.Response;
using RestSharp;

namespace Rapptor.Api.ApiCaller.RestSharp
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
			_apiClient.AddHandler("application/json", new JsonDotNetSerializer());
		}

		private static void ProcessRequestParameters(IRestRequest request, IEnumerable<RequestParameter> requestParameters)
		{
			foreach (var requestParameter in requestParameters)
			{
				request.AddParameter(requestParameter.Name, requestParameter.Value);
			}
		}

		public ResponseEnvelope<TReturn> ApiGet<TReturn>(string endpointToCall, string accessToken, params RequestParameter[] requestParameters) where TReturn : new()
		{
			var request = new RestRequest(endpointToCall, Method.GET)
				              {
					             RequestFormat = DataFormat.Json
								  , JsonSerializer = new JsonDotNetSerializer()
				              };
			request.AddParameter("access_token", accessToken ?? _accessToken);

			if (requestParameters != null)
				ProcessRequestParameters(request, requestParameters);

			var response = _apiClient.Execute<ResponseEnvelope<TReturn>>(request);

			if(response.ErrorException != null)
				throw response.ErrorException;

			if(response.ErrorMessage != null)
				throw new Exception(string.Format("Api Get of type {0} to endpoint {1} failed with message {2}", typeof(TReturn), API_BASE + endpointToCall, response.ErrorMessage));

			return response.Data;
		}

        public ResponseEnvelope<TReturn> ApiPost<TBody, TReturn>(string endpointToCall, string accessToken, TBody body = null, params RequestParameter[] requestParameters)
            where TReturn : new()
            where TBody : class, new()
		{
			var request = new RestRequest(endpointToCall, Method.POST)
				              {
					             RequestFormat = DataFormat.Json
								  , JsonSerializer = new JsonDotNetSerializer()
				              };
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Authorization", "Bearer " + (accessToken ?? _accessToken));	//wrapped in parenthesis in order to evaluate null check first.

			if(body != null)
				request.AddBody(body);

			if (requestParameters != null)
				ProcessRequestParameters(request, requestParameters);

			var response = _apiClient.Execute<ResponseEnvelope<TReturn>>(request);

			if (response.ErrorException != null)
				throw response.ErrorException;

			if (response.ErrorMessage != null)
				throw new Exception(string.Format("Api Post of type {0} to endpoint {1} failed with message {2}", typeof(TReturn), API_BASE + endpointToCall, response.ErrorMessage));

			return response.Data;
		}

		public ResponseEnvelope<TReturn> ApiPost<TReturn>(string endpointToCall, string accessToken, params RequestParameter[] requestParameters) where TReturn : new()
		{
			var response = ApiPost<object, TReturn>(endpointToCall, accessToken, requestParameters: requestParameters);

			return response;
		}

		public ResponseEnvelope<TReturn> ApiDelete<TReturn>(string endpointToCall, string accessToken) where TReturn : new()
		{
			var request = new RestRequest(endpointToCall, Method.DELETE) 
				              {
					             RequestFormat = DataFormat.Json
								  , JsonSerializer = new JsonDotNetSerializer()
				              };
			request.AddParameter("access_token", accessToken ?? _accessToken);

			var response = _apiClient.Execute<ResponseEnvelope<TReturn>>(request);

			if (response.ErrorException != null)
				throw response.ErrorException;

			if (response.ErrorMessage != null)
				throw new Exception(string.Format("Api Delete of type {0} to endpoint {1} failed with message {2}", typeof(TReturn), API_BASE + endpointToCall, response.ErrorMessage));

			return response.Data;
		}
	}
}