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
		public IEnumerable<string> UserIds { get; set; }
		public IEnumerable<string> Hashtags { get; set; }
		public IEnumerable<string> LinkDomains { get; set; }
		public IEnumerable<string> MentionUserIds { get; set; }
	}
}
