namespace Rapptor.Domain.Request
{
	public class PostStreamGeneralParameters
	{
		public string SinceId { get; set; }
		public string BeforeId { get; set; }
		public int? Count { get; set; }
		public bool? IncludeUser { get; set; }
		public bool? IncludeAnnotations { get; set; }
		public bool? IncludeReplies { get; set; }
	}
}
