using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouseService.Application.Common.Helpers
{
	public static class NameConverter
	{
		public static string ConvertToPascalCase(string snakeCase)
		{
			string[] words = snakeCase.Split('_');
			StringBuilder pascalCase = new StringBuilder();

			foreach (string word in words)
			{
				if (word.Length > 0)
				{
					pascalCase.Append(char.ToUpper(word[0]) + word.Substring(1));
				}
			}

			return pascalCase.ToString();
		}
	}
}
