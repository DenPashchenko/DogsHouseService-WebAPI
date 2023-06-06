using AutoMapper;
using DogsHouseService.Application.Common.Mappings;
using DogsHouseService.Application.Dogs.Commands;
using DogsHouseService.WebApi.Models.Validation;
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
		[Range(1, 200, ErrorMessage = "The value must be equal or greater than 1 and no more than 200.")]
		[JsonConverter(typeof(IntegerChecker))]
		public int TailLength { get; set; }

		[Range(1, 300, ErrorMessage = "The value must be equal or greater than 1 and no more than 300.")]
		[JsonConverter(typeof(IntegerChecker))]
		public int Weight { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<CreateDogDto, CreateDogCommand>();
		}
	}
}
