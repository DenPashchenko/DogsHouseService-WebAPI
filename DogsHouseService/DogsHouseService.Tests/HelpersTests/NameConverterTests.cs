using DogsHouseService.Application.Common.Helpers;

namespace DogsHouseService.Tests.HelpersTests
{
	public class NameConverterTests
	{
		[Theory]
		[InlineData("snake_case", "SnakeCase")]
		[InlineData("snake__case", "SnakeCase")]
		[InlineData("another_word_with_multiple_parts", "AnotherWordWithMultipleParts")]
		public void ConvertToPascalCase_ValidSnakeCase_ReturnsPascalCase(string snakeCase, string expectedPascalCase)
		{
			string result = NameConverter.ConvertToPascalCase(snakeCase);

			Assert.Equal(expectedPascalCase, result);
		}

		[Fact]
		public void ConvertToPascalCase_EmptyString_ReturnsEmptyString()
		{
			string snakeCase = string.Empty;

			string result = NameConverter.ConvertToPascalCase(snakeCase);

			Assert.Equal(string.Empty, result);
		}

		[Fact]
		public void ConvertToPascalCase_NullInput_ThrowsArgumentNullException()
		{
			string? snakeCase = null;

			Assert.Throws<ArgumentNullException>(() => NameConverter.ConvertToPascalCase(snakeCase));
		}

	}
}
