using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rapptor.Domain
{
	public class Filter
	{
		public Filter()
		{
			UserIds = new List<string>();
			Hashtags = new List<string>();
			LinkDomains = new List<string>();
			MentionUserIds = new List<string>();
		}

		public string Id { get; set; }
		public string Name { get; set; }
		public List<string> Hashtags { get; set; }
		
		[JsonProperty("user_ids")] public List<string> UserIds { get; set; }
		[JsonProperty("link_domains")] public List<string> LinkDomains { get; set; }
		[JsonProperty("mention_user_ids")] public List<string> MentionUserIds { get; set; }
	}
}
