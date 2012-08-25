using System.Collections.Generic;
using System.Threading;
using FubuTestingSupport;
using NUnit.Framework;
using Rapptor.Authorization;
using Rapptor.Domain.Authorization;

namespace Rapptor.Tests.Integration
{
	[TestFixture]
	public class AuthorizationSpec
	{
		private const string CLIENT_ID = "[INSERT CLIENT_ID HERE!]";
		private const string CLIENT_SECRET = "[INSERT CLIENT_SECRET HERE!]";
		private const string CLEARING_HOUSE_ADDRESS = "http://rapptorapp.net/";
		
		private AutoResetEvent _autoResetEvent;
		private List<Scope> _scopes;

		[SetUp]
		public void Setup()
		{
			_scopes = new List<Scope>
				          {
					          Scope.Stream
					          , Scope.Email
					          , Scope.WritePost
					          , Scope.Follow
					          , Scope.Messages
					          , Scope.Export
				          };
		}

		[Test]
		public void AuthorizationServiceCanConnectToClearingHouse()
		{
			//Setup
			var authorizationService = new AuthorizationService(CLEARING_HOUSE_ADDRESS, CLIENT_ID, CLIENT_SECRET, _scopes);
			
			//Execute
			var wasSuccessful = authorizationService.ConnectToClearingHouse();

			//Verify
			wasSuccessful.ShouldBeTrue();
			
			//Teardown
			authorizationService.DisconnectFromClearingHouse();
		}

		[Test]
		public void AuthorizationServiceCanDisconnectFromClearingHouse()
		{
			//Setup
			var authorizationService = new AuthorizationService(CLEARING_HOUSE_ADDRESS, CLIENT_ID, CLIENT_SECRET, _scopes);

			//Execute
			authorizationService.ConnectToClearingHouse();
			var wasSuccessful = authorizationService.DisconnectFromClearingHouse();

			//Verify
			wasSuccessful.ShouldBeTrue();

			//Teardown
		}

		[Test]
		public void AuthorizationServiceCanRetriveAccessToken()
		{
			//Setup
			this._autoResetEvent = new AutoResetEvent(false);
			var authorizationService = new AuthorizationService(CLEARING_HOUSE_ADDRESS, CLIENT_ID, CLIENT_SECRET, _scopes);

			authorizationService.AccessResponseReceived += accessResponse =>
			{
				accessResponse.AccessToken.ShouldNotBeNull();
				this._autoResetEvent.Set();
			};

			//Execute
			authorizationService.ConnectToClearingHouse();
			authorizationService.RetriveAccessToken();

			//Verify
			this._autoResetEvent.WaitOne();

			//Teardown
			authorizationService.DisconnectFromClearingHouse();
		}

		[Test]
		public void AuthorizationServiceCanSpecifyScopes()
		{
			//Setup
			this._autoResetEvent = new AutoResetEvent(false);
			var authorizationService = new AuthorizationService(CLEARING_HOUSE_ADDRESS, CLIENT_ID, CLIENT_SECRET, _scopes);

			authorizationService.AccessResponseReceived += accessResponse =>
			{
				accessResponse.AccessToken.ShouldNotBeNull();
				this._autoResetEvent.Set();
			};

			//Execute
			authorizationService.ConnectToClearingHouse();
			authorizationService.RetriveAccessToken();
			
			//Verify
			this._autoResetEvent.WaitOne();

			//Teardown
			authorizationService.DisconnectFromClearingHouse();
		}
	}
}
