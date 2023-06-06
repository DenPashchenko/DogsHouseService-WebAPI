using System.Text.Json.Serialization;
using System.Text.Json;

namespace DogsHouseService.WebApi.Models.Validation
{
	public class IntegerChecker : JsonConverter<int>
	{
		public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType is JsonTokenType.Number && reader.TryGetInt32(out int value))
			{
				return value;
			}

			throw new JsonException("The value must be an integer number.");
		}

		public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value);
		}
	}
}
