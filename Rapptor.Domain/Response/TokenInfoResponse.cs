using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rapptor.Domain.Response
{
	public class TokenInfoResponse
	{
		public List<string> Scopes { get; set; }
		public User User { get; set; }
        public Source App { get; set; }
        
        [JsonProperty("client_id")] public string ClientId { get; set; }
	}
}