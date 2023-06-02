using AutoMapper;
using DogsHouseService.Application.Interfaces;
using DogsHouseService.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouseService.Application.Dogs.Commands
{
	public class CreateDogCommandHandler : IRequestHandler<CreateDogCommand, int>
	{
		private readonly IAppDbContext _appDbContext;
		private readonly IMapper _mapper;

		public CreateDogCommandHandler(IAppDbContext appDbContext, IMapper mapper)
		{
			_appDbContext = appDbContext;
			_mapper = mapper;
		}

		public async Task<int> Handle(CreateDogCommand request, CancellationToken cancellationToken)
		{
			var dog = new Dog
			{
				Name = request.Name,
				Color = request.Color,
				TailLength = request.TailLength,
				Weight = request.Weight
			};

			await _appDbContext.Dogs.AddAsync(dog, cancellationToken);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return dog.Id;
		}
	}
}
