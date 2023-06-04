using DogsHouseService.Application.Common.Helpers;
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
		const int MaxPageSize = 50;
		public string SortingQuery { get; private set; }
		public int PageNumber { get; private set; } = 1;
		public int PageSize
		{
			get
			{
				return _pageSize;
			}
			set
			{
				_pageSize = (value > MaxPageSize) ? MaxPageSize : value;
			}
		}

		private string _sortingProperty = "Id";
		private string _orderBy = "ascending";
		private int _pageSize = 10;

		public GetDogListQuery(string? sortingProperty, string? orderBy, int? pageNumber, int? pageSize)
        {
			bool isValid = false;
			var propertyInfos = typeof(DogListDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			PageNumber = pageNumber ?? 1;
			PageSize = pageSize ?? _pageSize;

			if (!string.IsNullOrEmpty(orderBy) && orderBy.ToLower() == "desc")
            {
                _orderBy = "descending";
            }
			if (!string.IsNullOrEmpty(sortingProperty))
			{
				var normalizedSortingProperty = NameConverter.ConvertToPascalCase(sortingProperty);

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
					throw new ValidationException(Resources.InvalidSortingAttribute);
				}
			}

			SortingQuery = $"{_sortingProperty} {_orderBy}";
        }
	}
}
