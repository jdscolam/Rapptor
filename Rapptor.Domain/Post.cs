using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rapptor.Domain
{
	public class Post
	{
		public Post()
		{
			Annotations = new ConcurrentDictionary<string, dynamic>();
			Entities = new List<IEntity>();
		}

		public string Id { get; set; }
		public User User { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string Text { get; set; }
		public string Html { get; set; }
		public Source Source { get; set; }
		public string ReplyTo { get; set; }
		public string ThreadId { get; set; }
		public int? NumReplies { get; set; }
		public IDictionary<string, dynamic> Annotations { get; set; }
		public IEnumerable<IEntity> Entities { get; set; }
		public bool? IsDeleted { get; set; }
	}
}
