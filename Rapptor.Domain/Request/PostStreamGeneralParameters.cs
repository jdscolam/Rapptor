namespace Rapptor.Domain.Request
{
	public class PostStreamGeneralParameters
	{
		public string SinceId { get; set; }
		public string BeforeId { get; set; }
		public int? Count { get; set; }
		public int? IncludeMuted { get; set; }
		public int? IncludeDeleted { get; set; }
		public int? IncludeDirectedPosts { get; set; }
		public int? IncludeAnnotations { get; set; }
		public int? IncludeMachine { get; set; }
		public int? IncludeReplies { get; set; }
        public int? IncludeUser { get; set; }
        public int? IncludeStarredBy { get; set; }
        public int? IncludeReposters { get; set; }
	}
}
