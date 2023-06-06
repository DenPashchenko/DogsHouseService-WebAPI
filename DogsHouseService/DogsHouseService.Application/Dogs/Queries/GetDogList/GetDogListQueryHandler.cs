using AutoMapper;
using AutoMapper.QueryableExtensions;
using DogsHouseService.Application.Common.Helpers;
using DogsHouseService.Application.Interfaces;
using MediatR;
using System.Linq.Dynamic.Core;

namespace DogsHouseService.Application.Dogs.Queries.GetDogList
{
	public class GetDogListQueryHandler : IRequestHandler<GetDogListQuery, DogListVm>
	{
		private readonly IAppDbContext _appDbContext;
		private readonly IMapper _mapper;

		public GetDogListQueryHandler(IAppDbContext dataDbContext, IMapper mapper)
		{
			_appDbContext = dataDbContext;
			_mapper = mapper;
		}

		public async Task<DogListVm> Handle(GetDogListQuery request, CancellationToken cancellationToken)
		{
			var dogsQuery = await PagedList<DogListDto>
				.ToPagedListAsync(_appDbContext.Dogs
					.ProjectTo<DogListDto>(_mapper.ConfigurationProvider)
					.OrderBy(request.SortingQuery),
						request.PageNumber, request.PageSize, cancellationToken);

			var metadata = new Dictionary<string, object>
			{
				{ "TotalCount", dogsQuery.TotalCount },
				{ "PageSize", dogsQuery.PageSize },
				{ "CurrentPage", dogsQuery.CurrentPage },
				{ "TotalPages", dogsQuery.TotalPages },
				{ "HasNext", dogsQuery.HasNext },
				{ "HasPrevious", dogsQuery.HasPrevious }
			};

			return new DogListVm { Dogs = dogsQuery, Metadata = metadata };
		}
	}
}
