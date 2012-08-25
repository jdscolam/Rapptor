using System.Collections.Generic;

namespace Rapptor.Domain.Response
{
	public class TokenInfoResponse
	{
		public List<string> Scopes { get; set; }
		public User User { get; set; }
	}
}