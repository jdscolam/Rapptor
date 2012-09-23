using System;
using Newtonsoft.Json;

namespace Rapptor.Domain
{
	public class Source
	{
		public string Name { get; set; }
		public Uri Link { get; set; }
		
        [JsonProperty("client_id")] public string ClientId { get; set; }
	}
}