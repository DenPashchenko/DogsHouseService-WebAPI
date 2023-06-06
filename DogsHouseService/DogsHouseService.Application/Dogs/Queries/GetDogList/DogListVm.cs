namespace DogsHouseService.Application.Dogs.Queries.GetDogList
{
	public class DogListVm
	{
		public IList<DogListDto>? Dogs { get; set; }

		public Dictionary<string, object> Metadata { get; set; } = null!;
	}
}
