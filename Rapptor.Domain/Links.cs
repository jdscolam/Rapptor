using System;

namespace Rapptor.Domain
{
	public class Links : IEntity
	{
		public string Text { get; set; }
		public Uri Url { get; set; }
		public int Pos { get; set; }
		public int Len { get { return !string.IsNullOrWhiteSpace(Text) ? Text.Length + 1 : 0; } }
	}
}