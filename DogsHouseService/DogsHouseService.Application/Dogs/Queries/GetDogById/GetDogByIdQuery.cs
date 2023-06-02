using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouseService.Application.Dogs.Queries.GetDogById
{
	public class GetDogByIdQuery : IRequest<DogVm>
	{
		public int Id { get; set; }
	}
}
