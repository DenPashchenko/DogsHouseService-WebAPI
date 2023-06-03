using DogsHouseService.Application.Properties;
using DogsHouseService.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouseService.Application.Dogs.Queries.GetDogList
{
	public class GetDogListQuery : IRequest<DogListVm>
	{
        public string SortingQuery { get; private set; }

		private string _sortingProperty = "Id";
		private string _orderBy = "ascending";

		public GetDogListQuery(string sortingProperty, string orderBy)
        {
			bool isValid = false;
			var propertyInfos = typeof(DogListDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			if (!string.IsNullOrEmpty(orderBy) && orderBy.ToLower() == "desc")
            {
                _orderBy = "descending";
            }
			if (!string.IsNullOrEmpty(sortingProperty))
			{
				var normalizedSortingProperty = NormalizeSortingProperty(sortingProperty);

				foreach (var property in propertyInfos)
				{
					if (property.Name == normalizedSortingProperty)
					{
						_sortingProperty = property.Name;
						isValid = true;
						break;
					}
				}
				if (!isValid)
				{
					throw new ValidationException("Invalid attribute.");
				}
			}

			SortingQuery = $"{_sortingProperty} {_orderBy}";
        }

		private string NormalizeSortingProperty(string sortingProperty)
		{
			string[] words = sortingProperty.Split('_');
			StringBuilder normalizedProperty = new StringBuilder();

			foreach (string word in words)
			{
				if (word.Length > 0)
				{
					normalizedProperty.Append(char.ToUpper(word[0]) + word.Substring(1));
				}
			}

			return normalizedProperty.ToString();
		}
	}
}
