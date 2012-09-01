using System.Dynamic;
using Newtonsoft.Json;

namespace Rapptor.Domain
{
	public class Annotation
	{
		[JsonProperty("type")] public string Type { get; set; }
		
		//This attribute is CRUCIAL!  It enables fully dynamic attributes on App.net using a Json.NET trick.
		[JsonProperty("value", TypeNameHandling = TypeNameHandling.All, ItemTypeNameHandling = TypeNameHandling.All)] 
		public dynamic Value { get; set; }
	}
}
