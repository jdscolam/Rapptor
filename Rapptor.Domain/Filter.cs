using System.Collections.Generic;

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
		public List<string> UserIds { get; set; }
		public List<string> Hashtags { get; set; }
		public List<string> LinkDomains { get; set; }
		public List<string> MentionUserIds { get; set; }
	}
}
