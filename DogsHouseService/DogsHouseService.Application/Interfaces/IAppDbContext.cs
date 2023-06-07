using DogsHouseService.Domain;
using Microsoft.EntityFrameworkCore;

namespace DogsHouseService.Application.Interfaces
{
	public interface IAppDbContext
	{
		DbSet<Dog> Dogs { get; set; }

		Task<int> SaveChangesAsync (CancellationToken cancellationToken);
	}
}
