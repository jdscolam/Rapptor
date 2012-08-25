using System;
using System.Drawing;

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
	    public WebImage AvatarImage { get; set; }
	    public WebImage CoverImage { get; set; }
	    public string Type { get; set; }
	    public DateTime? CreatedAt { get; set; }
	    public Counts Counts { get; set; }
	    public dynamic AppData { get; set; }
	    public bool? FollowsYou { get; set; }
	    public bool? YouFollow { get; set; }
	    public bool? YouMuted { get; set; }
    }
}
