using AutoMapper;
using DogsHouseService.Application.Common.Mappings;
using DogsHouseService.Domain;
using System.Text.Json.Serialization;

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

		public override bool Equals(object? obj)
		{
			if (obj == null || GetType() != obj.GetType())
				return false;

			DogListDto other = (DogListDto)obj;

			return Id == other.Id &&
				   Name == other.Name &&
				   Color == other.Color &&
				   TailLength == other.TailLength &&
				   Weight == other.Weight;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = 17;
				hashCode = hashCode * 23 + Id.GetHashCode();
				hashCode = hashCode * 23 + (Name != null ? Name.GetHashCode() : 0);
				hashCode = hashCode * 23 + (Color != null ? Color.GetHashCode() : 0);
				hashCode = hashCode * 23 + TailLength.GetHashCode();
				hashCode = hashCode * 23 + Weight.GetHashCode();
				return hashCode;
			}
		}
	}
}
