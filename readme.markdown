Rapptor
--

Freedom in the key of C#, or a C# implementation of the App.net API.

Release Notes
--

**Version 0.3**
- Added TokenSpec
- Added RestSharpApiCallerSpec
- Added Raptor.Api
- Fixed Domain issues.

**Version 0.2.1 (not released on NuGet)**
- Added configurable Scopes to Authorization Spec

**Version 0.2**
- Added Authorization Spec (this is HUGE!)

**Version 0.1**
- Initial Release
- Added a basic implementation of all key App.net objects.

Using Rapptor.Api
--

1. Authenticate first to ensure you have an `access_token`
1. Get Raptor.Api from NuGet
1. Initialize an instance of the appropriate API service while passing it an `IApiCaller` (e.g. `TokenService(new RestSharpApiCaller(access_token))`)
1. Call the appropriate API method (e.g. `tokenService.RetriveCurrentTokenInfo();`)

Using Rapptor.Authentication
--

1. Get Raptor.Authentication from NuGet
1. Initialize an instance of the `AuthorizationService` passing in the appropriate parameters
1. Subscribe to the AccessResponseReceived event (or provide a callback to the `RetrieveAccessToken(...)` call)
1. Call `authorizationService.ConnectToClearingHouse();`
1. Call `authorizationService.RetriveAccessToken();`
1. Enjoy!

**NOTE: Authentication will open a browser window in another process so the user can authorize the application on App.net!**

**Example from AuthorizationSpec.cs**

var scopes = new List<Scope>
				          {
					          Scope.Stream
					          , Scope.Email
					          , Scope.WritePost
					          , Scope.Follow
					          , Scope.Messages
					          , Scope.Export
				          };

var authorizationService = new AuthorizationService(CLEARING_HOUSE_ADDRESS, CLIENT_ID, CLIENT_SECRET, scopes);

authorizationService.AccessResponseReceived += accessResponse =>
{
	myAccessToken = accessResponse.AccessToken;
};

authorizationService.ConnectToClearingHouse();
authorizationService.RetriveAccessToken();

Using Rapptor.Domain
--

1. Get Raptor.Domain from NuGet
1. Add `using Rapptor.Domain;` to your class
1. Use the included classes when interfacing with the App.net API

Included Objects
--

1. User
1. Description
1. Counts
1. IEntity (Mentions, Hashtags, Links)
1. Post
1. Source
1. Filter

Input Needed (and Pull-Requests Accepted)
--

1. Any suggestions on how to avoid showing a browser for authentication.
1. Thoughts on using `Post` objects as references instead of `id` when dealing with threads
1. Possible better implementations when handling images with `Image`

Coming Soon
--

1. More API calls with tests
1. Async authorization calls
1. Example app
1. More cool stuff

Credit Where it is Due
--

SignalR Clearing House Hub curtousey of Frank Wanicka, @fwanicka

Bottom Line
--

We shall be slaves no longer!