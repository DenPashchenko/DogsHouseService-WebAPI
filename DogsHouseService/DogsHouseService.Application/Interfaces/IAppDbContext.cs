using DogsHouseService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouseService.Application.Interfaces
{
	public interface IAppDbContext
	{
		DbSet<Dog> Dogs { get; set; }

		Task<int> SaveChangesAsync (CancellationToken cancellationToken);
	}
}
