using DogsHouseService.Application.Common.Helpers;
using DogsHouseService.Application.Properties;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DogsHouseService.Application.Dogs.Queries.GetDogList
{
	public class GetDogListQuery : IRequest<DogListVm>
	{
		private const string DefaultAttribute = "Id";

		public string SortingQuery
		{
			get => _sortingQuery;
		}
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public string Attribute { get; set; } = null!;
		public string Order { get; set; } = null!;

		private string _sortingQuery => GetSortingQuery();

		private string GetSortingQuery()
		{
			bool isValid = false;
			var propertyInfos = typeof(DogListDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);

			if (!string.IsNullOrEmpty(Order) && (Order.ToLower() == "desc" || Order.ToLower() == "descending"))
			{
				Order = "descending";
			}
			else
			{
				Order = "ascending";
			}
			if (!string.IsNullOrEmpty(Attribute))
			{
				var normalizedSortingProperty = NameConverter.ConvertToPascalCase(Attribute);

				foreach (var property in propertyInfos)
				{
					if (property.Name == normalizedSortingProperty)
					{
						Attribute = property.Name;
						isValid = true;
						break;
					}
				}
				if (!isValid)
				{
					throw new ValidationException(Resources.InvalidSortingAttribute);
				}
			}
			else
			{
				Attribute = DefaultAttribute;
			}

			return $"{Attribute} {Order}";
		}
	}
}
