using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouseService.Application.Dogs.Commands
{
	public class CreateDogCommand : IRequest<int>
	{
		public string Name { get; set; } = null!;

		public string Color { get; set; } = null!;

		public int TailLength { get; set; }

		public int Weight { get; set; }
	}
}
