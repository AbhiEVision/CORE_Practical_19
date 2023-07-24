using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Practical_19.Enum
{
	[JsonConverter(typeof(StringEnumConverter))]
	public enum Roles
	{
		User,
		Admin
	}
}
