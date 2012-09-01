using System;
using Newtonsoft.Json;

namespace Rapptor.Domain
{
    public class User
    {
	    public string Id { get; set; }
	    public string Username { get; set; }
	    public string Name { get; set; }
	    public Description Description { get; set; }
		public string Timezone { get; set; }
	    public string Locale { get; set; }
	    public string Type { get; set; }
	    public Counts Counts { get; set; }
	    
		[JsonProperty("avatar_image")] public WebImage AvatarImage { get; set; }
		[JsonProperty("cover_image")] public WebImage CoverImage { get; set; }
		[JsonProperty("created_at")] public DateTime? CreatedAt { get; set; }
	    [JsonProperty("app_data")] public dynamic AppData { get; set; }
		[JsonProperty("follows_you")] public bool? FollowsYou { get; set; }
		[JsonProperty("you_follow")] public bool? YouFollow { get; set; }
		[JsonProperty("you_muted")] public bool? YouMuted { get; set; }
    }
}
