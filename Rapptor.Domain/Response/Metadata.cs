using Newtonsoft.Json;

namespace Rapptor.Domain.Response
{
	public class Metadata
	{
		public string Code { get; set; }
		[JsonProperty("error_slug")] public string ErrorSlug { get; set; }
		[JsonProperty("error_message")] public string ErrorMessage { get; set; }
		[JsonProperty("max_id")] public string MaxId { get; set; }
		[JsonProperty("min_id")] public string MinId { get; set; }
		public bool? More { get; set; }
	}
}