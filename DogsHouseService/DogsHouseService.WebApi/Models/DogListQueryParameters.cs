using AutoMapper;
using DogsHouseService.Application.Common.Mappings;
using DogsHouseService.Application.Dogs.Queries.GetDogList;
using DogsHouseService.WebApi.Models.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DogsHouseService.WebApi.Models
{
	public class DogListQueryParameters : IMapWith<GetDogListQuery>
	{
		private const int MaxPageSize = 50;
		private const int DefaultPageNumber = 1;
		private const int DefaultPageSize = 10;
				
		public string? Attribute { get; set; }

		public string? Order { get; set; }

		[Range(1, int.MaxValue)]
		[JsonConverter(typeof(IntegerValidator))]
		public int PageNumber { get; set; } = DefaultPageNumber;

		[Range(1, int.MaxValue)]
		[JsonConverter(typeof(IntegerValidator))]
		public int PageSize
		{
			get => _pageSize;
			set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
		}

		private int _pageSize = DefaultPageSize;

		public void Mapping(Profile profile)
		{
			profile.CreateMap<DogListQueryParameters, GetDogListQuery>();
		}
	}
}
