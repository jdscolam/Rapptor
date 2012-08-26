namespace Rapptor.Domain
{
	public interface IApiCaller
	{
		T ApiGet<T>(string endpointToCall) where T : new();
		T ApiPost<T>(string endpointToCall) where T : new();
		T ApiDelete<T>(string endpointToCall) where T : new();
	}
}