using AutoMapper;
using DogsHouseService.Application.Common.Mappings;
using DogsHouseService.Application.Dogs.Commands;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DogsHouseService.WebApi.Models
{
	public class CreateDogDto : IMapWith<CreateDogCommand>
	{
		[Required, StringLength(20, ErrorMessage = "Name property's length can't be more than 20 characters.")]
		public string Name { get; set; } = null!;

		[Required, StringLength(30, ErrorMessage = "Color property's length can't be more than 30 characters.")]
		public string Color { get; set; } = null!;

		[JsonPropertyName("tail_length")]
		[Range(0, 2_000, ErrorMessage = "The value must be equal or greater than 0 and no more than 2 000.")]
		public int TailLength { get; set; }

		[Range(1, 100_000, ErrorMessage = "The value must be equal or greater than 1 and no more than 100 000.")]
		public int Weight { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<CreateDogDto, CreateDogCommand>();
		}
	}
}
