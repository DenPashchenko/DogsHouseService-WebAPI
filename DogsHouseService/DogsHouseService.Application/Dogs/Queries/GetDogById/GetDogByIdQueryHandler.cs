using AutoMapper;
using DogsHouseService.Application.Common.Exceptions;
using DogsHouseService.Application.Interfaces;
using DogsHouseService.Domain;
using MediatR;

namespace DogsHouseService.Application.Dogs.Queries.GetDogById
{
	public class GetDogByIdQueryHandler : IRequestHandler<GetDogByIdQuery, DogVm>
	{
		private readonly IAppDbContext _appDbContext;
		private readonly IMapper _mapper;

		public GetDogByIdQueryHandler(IAppDbContext appDbContext, IMapper mapper)
		{
			_appDbContext = appDbContext;
			_mapper = mapper;
		}

		public async Task<DogVm> Handle(GetDogByIdQuery request, CancellationToken cancellationToken)
		{
			var dog = await _appDbContext.Dogs.FindAsync(request.Id);
			if (dog == null)
			{
				throw new NotFoundException(nameof(Dog), request.Id);
			}

			return _mapper.Map<DogVm>(dog);
		}
	}
}
