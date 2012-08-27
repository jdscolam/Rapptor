using System;

namespace Rapptor.Domain
{
	public class Link : IEntity
	{
		public string Text { get; set; }
		public string Url { get; set; }
		public int Pos { get; set; }
		public int Len { get { return !string.IsNullOrWhiteSpace(Text) ? Text.Length + 1 : 0; } }
	}
}