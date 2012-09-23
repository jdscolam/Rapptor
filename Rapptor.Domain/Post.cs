using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Rapptor.Domain
{
	public class Post
	{
		public Post()
		{
			Annotations = new List<Annotation>();
		}

		public string Id { get; set; }
		public User User { get; set; }
		public string Text { get; set; }
		public string Html { get; set; }
		public Source Source { get; set; }
		public List<Annotation> Annotations { get; set; }
		public Entities Entities { get; set; }
		public List<User> Reposters { get; set; }
		
		[JsonProperty("created_at")] public DateTime? CreatedAt { get; set; }
		[JsonProperty("reply_to")] public string ReplyTo { get; set; }
		[JsonProperty("thread_id")] public string ThreadId { get; set; }
		[JsonProperty("num_replies")] public int? NumReplies { get; set; }
        [JsonProperty("num_stars")] public int? NumStars { get; set; }
        [JsonProperty("num_reposts")] public int? NumReposts { get; set; }
		[JsonProperty("is_deleted")] public bool? IsDeleted { get; set; }
		[JsonProperty("machine_only")] public bool? MachineOnly { get; set; }
		[JsonProperty("you_starred")] public bool? YouStarred { get; set; }
		[JsonProperty("you_reposted")] public bool? YouReposted { get; set; }
		[JsonProperty("starred_by")] public List<User> StarredBy { get; set; }
		[JsonProperty("repost_of")] public Post RepostOf { get; set; }
	}
}
