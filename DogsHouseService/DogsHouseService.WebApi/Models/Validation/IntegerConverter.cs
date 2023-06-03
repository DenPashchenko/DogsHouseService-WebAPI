using System.Text.Json.Serialization;
using System.Text.Json;

namespace DogsHouseService.WebApi.Models.Validation
{
	public class IntegerConverter : JsonConverter<int>
	{
		public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType is JsonTokenType.Number && reader.TryGetInt32(out int value))
			{
				return value;
			}

			throw new JsonException("The value must be a positive integer number.");
		}

		public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
		{
			writer.WriteNumberValue(value);
		}
	}
}
