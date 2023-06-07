using AutoMapper;
using DogsHouseService.Application.Common.Mappings;
using DogsHouseService.Domain;
using System.Text.Json.Serialization;

namespace DogsHouseService.Application.Dogs.Queries.GetDogById
{
	public class DogVm : IMapWith<Dog>
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public string Color { get; set; } = null!;

		[JsonPropertyName("tail_length")]
		public int TailLength { get; set; }

		public int Weight { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<Dog, DogVm>();
		}
	}
}
