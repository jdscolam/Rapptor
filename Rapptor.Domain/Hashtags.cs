namespace Rapptor.Domain
{
	public class Hashtags : IEntity
	{
		public string Name { get; set; }
		public int Pos { get; set; }
		public int Len { get { return !string.IsNullOrWhiteSpace(Name) ? Name.Length + 1 : 0; } }
	}
}