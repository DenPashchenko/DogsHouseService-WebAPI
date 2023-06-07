using MediatR;

namespace DogsHouseService.Application.Dogs.Queries.GetDogById
{
	public class GetDogByIdQuery : IRequest<DogVm>
	{
		public int Id { get; set; }
	}
}
