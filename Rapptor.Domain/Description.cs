using System.Collections.Generic;

namespace Rapptor.Domain
{
	public class Description
	{
		public Description()
		{
			Entities = new List<IEntity>();
		}

		public string Text { get; set; }
		public string Html { get; set; }
		public IEnumerable<IEntity> Entities { get; set; }
	}
}