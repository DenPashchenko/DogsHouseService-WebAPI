using AutoMapper;
using AutoMapper.QueryableExtensions;
using DogsHouseService.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

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
			var dogsQuery = await _appDbContext.Dogs
				.ProjectTo<DogListDto>(_mapper.ConfigurationProvider)
				.OrderBy(request.SortingQuery)
				.ToListAsync(cancellationToken);

			return new DogListVm { Dogs = dogsQuery };
		}
	}
}
