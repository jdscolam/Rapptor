using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Rapptor.Domain.Authorization;
using RestSharp;
using SignalR.Client.Hubs;

namespace Rapptor.Authorization
{
	public class AuthorizationService
	{
		private const string HUB_NAME = "AppNetHub";
		private const string GRANT_TYPE = "authorization_code";
		private const string REDIRECT_PATH = "/appnet/authorizationcallback";
		
		private const string REGISTER_COMPLETE_EVENT_NAME = "registerComplete";
		private const string AUTHORIZATION_COMPLETE_EVENT_NAME = "authorizationComplete";

		public Action<AccessResponse> AccessResponseReceived;

		private readonly HubConnection _hubConnection;
		private readonly IHubProxy _hubProxy;
		private readonly string _clientId;
		private readonly string _redirectUri;
		private readonly string _clientSecret;
		private readonly IEnumerable<Scope> _scopes;

		public AuthorizationService(string clearingHouseAddress, string clientId, string clientSecret, IEnumerable<Scope> scopes)
		{
			if(!Uri.IsWellFormedUriString(clearingHouseAddress, UriKind.Absolute))
				throw new ArgumentException(string.Format("Cannot create AuthorizationService, Clearing House Uri {0} is not well formed.", clearingHouseAddress));

			var cleanedClearingHouseAddress = clearingHouseAddress.Last() == '/' ? clearingHouseAddress.Substring(0, clearingHouseAddress.Length - 1) : clearingHouseAddress;

			_clientId = clientId;
			_clientSecret = clientSecret;
			_scopes = scopes;
			_redirectUri = cleanedClearingHouseAddress + REDIRECT_PATH;
			_hubConnection = new HubConnection(clearingHouseAddress);
			_hubProxy = _hubConnection.CreateProxy(HUB_NAME);

			InitializeHubProxy();
		}

		private void InitializeHubProxy()
		{
			_hubProxy.On<String>(REGISTER_COMPLETE_EVENT_NAME, RegistrationCompleted);

			_hubProxy.On<String>(AUTHORIZATION_COMPLETE_EVENT_NAME, AuthorizationCompleted);
		}

		private void RegistrationCompleted(string returnedConnectionId)
		{
			//Build Authentication URI.
			var uri = @"https://alpha.app.net/oauth/authenticate?client_id=" + _clientId 
				+ "&response_type=code&redirect_uri=" + _redirectUri
				+ "&scope=" + string.Join(" ", _scopes.Select(x => x.Name))
				+ "&state=" + returnedConnectionId;

			//Launch the default browser with a given URI.
			//TODO: This is a "bit" #hackey, and could probably be done better by scraping the "Authorization" response, and if access has already been granted skipping this step all together.
			var process = new Process
				              {
					              EnableRaisingEvents = false, StartInfo = {FileName = uri}
				              };

			process.Start();
		}

		private void AuthorizationCompleted(string authenticationCode)
		{
			//Using RestSharp, build a URL encoded post statement.
			var client = new RestClient("https://alpha.app.net");

			var tokenRequest = new RestRequest(@"/oauth/access_token", Method.POST);

			tokenRequest.AddParameter("client_id", _clientId);
			tokenRequest.AddParameter("client_secret", _clientSecret);
			tokenRequest.AddParameter("grant_type", GRANT_TYPE);
			tokenRequest.AddParameter("redirect_uri", _redirectUri);
			tokenRequest.AddParameter("code", authenticationCode);

			var response = client.Execute<AccessResponse>(tokenRequest);

			OnAccessResponseReceived(response.Data);
		}

		private void OnAccessResponseReceived(AccessResponse accessResponse)
		{
			if(AccessResponseReceived != null)
				AccessResponseReceived(accessResponse);
		}

		/// <summary>
		/// Connects to the configured clearing house
		/// </summary>
		public bool ConnectToClearingHouse()
		{
			var task = _hubConnection.Start();
			task.Wait();

			if(task.IsFaulted)
			{
				var message = string.Format("Connection to clearing house at {0} Failed!  ", _hubConnection.Url);
				throw task.Exception != null ? new AggregateException(message + task.Exception.Message, task.Exception) : new Exception(message + "Task faulted without aggregate exception.");
				
			}

			if(task.IsCanceled)
				throw new TaskCanceledException(string.Format("Connection to clearing house at {0} Failed!  Task was cancelled.", _hubConnection.Url));

			return _hubConnection.ConnectionId != null;
		}

		/// <summary>
		/// Retrieve the access token for the user.
		/// 
		/// WARNING:  This will launch a browser window
		/// </summary>
		/// <param name="callback"></param>
		public void RetriveAccessToken(Action<AccessResponse> callback = null)
		{
			//If a callback has been specified, register it.
			if(callback != null)
				AccessResponseReceived += callback;

			_hubProxy.Invoke("Register", "").Wait();
		}
	}
}