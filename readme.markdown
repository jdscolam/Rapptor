Rapptor
--

Freedom in the key of C#, or a C# implementation of the App.net API.

Release Notes
--

**Machine Only - Version 0.5**
- Added Machine only posts and post filter

**ANNOTATIONS! - Version 0.5**
- Added fully dynamic Post Annotations
- NOTICE: Added dependency on Json.Net (unavoidable without lots of extra work)
- Added ResponseEnvelope to RestSharpApiCaller
- Fixed minor Domain issues

**HUGE UPDATE - Version 0.4**
- Added full UsersSpec
- Added full PostsSpec
- Added integration tests for Posts and Users
- Fixed minor Domain issues

**Version 0.3**
- Added TokenSpec
- Added RestSharpApiCallerSpec
- Added Rapptor.Api
- Fixed Domain issues

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
1. Get Rapptor.Api from NuGet
1. Initialize an instance of the appropriate API service while passing it an `IApiCaller` (e.g. `TokenService(new RestSharpApiCaller(access_token))`)
1. Call the appropriate API method (e.g. `tokenService.RetriveCurrentTokenInfo();`)

Using Post Annotations
--

1. Create a new `Annotation`
1. Set it's `Type` equal to the reversed url of your choice 
1. Set it's `Value` property to whatever class/value you want (`Annotations` are limited to 8k)
1. Add your new `Annotation` to a `List<Annotation>`
1. Create a new `CreatePostRequest` and set it's `Annotations` property to your `List<Annotation`
1. Call `postService.CreatePost` with your `CreatePostRequest`
1. Call `postService.Get(postId)` to retrieve your post with its new annotation.

***NOTE: Some Types are reserved, see the API for more information: https://github.com/appdotnet/api-spec/blob/master/annotations.md***

***Annotations Example***

```c#
var annotationValue = new MyAnnotationClass
				            {
					            Name = "My test parameter annotation"
								, Value = 23.5M
				            };
var annotation = new Annotation
				        {
					        Type = "net.raptorapp.test.request.parameter"
							, Value = annotationValue
				        };

var createPostRequest = new CreatePostRequest
{
	Text = @"@jdscolam this is another #Rapptor #testpost, with links and stuff.  https://github.com/jdscolam/Rapptor and Rapptor NuGet"
	, ReplyTo = "197934"
	, Annotations = new List<Annotation> { annotation }
};

var myNewPost = postService.CreatePost(createPostRequest);
```

Using Rapptor.Authorization
--

1. Get Rapptor.Authorization from NuGet
1. Initialize an instance of the `AuthorizationService` passing in the appropriate parameters
1. Subscribe to the AccessResponseReceived event (or provide a callback to the `RetrieveAccessToken(...)` call)
1. Call `authorizationService.ConnectToClearingHouse();`
1. Call `authorizationService.RetriveAccessToken();`
1. Enjoy!

**NOTE: Authentication will open a browser window in another process so the user can authorize the application on App.net!**

**Example from AuthorizationSpec.cs**
```c#
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
```

Using Rapptor.Domain
--

1. Get Rapptor.Domain from NuGet
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
1. Any thoughts on cleaning up current code as it stands.
1. Ideas on Filters, Streams, and Subscriptions implementation.

Coming Soon
--

1. More API calls as they are delivered by App.net team
1. Async authorization calls
1. Example app
1. Super secret feature

Credit Where it is Due
--

SignalR Clearing House Hub curtousey of Frank Wanicka, @fwanicka
JsonDotNetSerializer inital version Copyright 2010 John Sheehan

Bottom Line
--

We shall be slaves no longer!