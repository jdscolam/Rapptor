namespace Rapptor.Domain.Response
{
	public class ResponseEnvelope<T> where T : new()
	{
		public T Data { get; set; }
		public Metadata Meta { get; set; }
	}
}
