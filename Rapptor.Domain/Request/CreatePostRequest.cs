using System.Collections.Generic;

namespace Rapptor.Domain.Request
{
	public class CreatePostRequest
	{
		public CreatePostRequest()
		{
			Links = new List<Link>();
		}

		public string Text { get; set; }
		public string ReplyTo { get; set; }
		public dynamic Annotations { get; set; }
		public List<Link> Links { get; set; }
	}
}
