using AutoMapper;
using DogsHouseService.Application.Common.Mappings;
using DogsHouseService.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DogsHouseService.Application.Dogs.Queries.GetDogList
{
	public class DogListDto : IMapWith<Dog>
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public string Color { get; set; } = null!;

		[JsonPropertyName("tail_length")]
		public int TailLength { get; set; }

		public int Weight { get; set; }

		public void Mapping(Profile profile)
		{
			profile.CreateMap<Dog, DogListDto>();
		}
	}
}
