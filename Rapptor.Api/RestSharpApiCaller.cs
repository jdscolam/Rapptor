using System;
using Rapptor.Domain;
using RestSharp;

namespace Rapptor.Api
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

		public T ApiGet<T>(string endpointString) where T : new()
		{
			var request = new RestRequest(endpointString, Method.GET) {RootElement = "data"};
			request.AddParameter("access_token", _accessToken);

			var response = _apiClient.Execute<T>(request);

			if(response.ErrorException != null)
				throw response.ErrorException;

			if(response.ErrorMessage != null)
				throw new Exception(string.Format("Api Get of type {0} to endpoint {1} failed with message {2}", typeof(T), API_BASE + endpointString, response.ErrorMessage));

			return response.Data;
		}
	}
}