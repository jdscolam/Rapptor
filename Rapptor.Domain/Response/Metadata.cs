namespace Rapptor.Domain.Response
{
	public class Metadata
	{
		public string Code { get; set; }
		public string ErrorSlug { get; set; }
		public string ErrorMessage { get; set; }
		public string MaxId { get; set; }
		public string MinId { get; set; }
		public bool? More { get; set; }
	}
}