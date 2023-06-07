using AutoMapper;
using DogsHouseService.Application.Common.Mappings;
using DogsHouseService.Application.Dogs.Queries.GetDogList;
using DogsHouseService.WebApi.Models.Abstractions;

namespace DogsHouseService.WebApi.Models
{
	public class DogListQueryParameters : QueryStringParameters, IMapWith<GetDogListQuery>
	{
		public void Mapping(Profile profile)
		{
			profile.CreateMap<DogListQueryParameters, GetDogListQuery>();
		}
	}
}
