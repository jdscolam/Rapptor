using System.Collections.Generic;

namespace Rapptor.Domain
{
	public class Entities
	{
		public Entities()
		{
			Hashtags = new List<Hashtag>();
			Links = new List<Link>();
			Mentions = new List<Mention>();
		}

		public List<Hashtag> Hashtags { get; set; }
		public List<Link> Links { get; set; }
		public List<Mention> Mentions { get; set; }
	}
}