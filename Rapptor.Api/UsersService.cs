using Rapptor.Domain;

namespace Rapptor.Api
{
	public class UsersService
	{
		public const string USERS_ENDPOINT = "users/";

		private readonly IApiCaller _apiCaller;

		public UsersService(IApiCaller apiCaller)
		{
			_apiCaller = apiCaller;
		}

		public User RetrieveUser(string userId)
		{
			var user = _apiCaller.ApiGet<User>(USERS_ENDPOINT + userId);

			return user;
		}
	}
}