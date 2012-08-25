namespace Rapptor.Domain
{
	public interface IApiCaller
	{
		T ApiGet<T>(string endpointString) where T : new();
	}
}