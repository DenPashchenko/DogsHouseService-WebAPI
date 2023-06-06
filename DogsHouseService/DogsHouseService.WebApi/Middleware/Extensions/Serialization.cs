using System.Text;
using System.Text.Json;

namespace DogsHouseService.WebApi.Middleware.Extensions
{
	public static class Serialization
	{
		public static byte[] ToByteArray(this object objectToSerialize)
		{
			ArgumentNullException.ThrowIfNull(objectToSerialize);
			
			return Encoding.Default.GetBytes(JsonSerializer.Serialize(objectToSerialize));
		}

		public static T FromByteArray<T>(this byte[] arrayToDeserialize) where T : class
		{
			if (arrayToDeserialize == null)
			{
				return default(T);
			}

			return JsonSerializer.Deserialize<T>(Encoding.Default.GetString(arrayToDeserialize));
		}
	}
}
