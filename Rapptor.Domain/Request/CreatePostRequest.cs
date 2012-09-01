using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rapptor.Domain.Request
{
	public class CreatePostRequest
	{
		public CreatePostRequest()
		{
			Annotations = new List<Annotation>();
		}

		[JsonProperty("text")] public string Text { get; set; }
		[JsonProperty("reply_to")] public string ReplyTo { get; set; }
		[JsonProperty("annotations")] public List<Annotation> Annotations { get; set; }
	}
}
