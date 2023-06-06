using DogsHouseService.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DogsHouseService.Persistence
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
		{
			var connectionString = configuration.GetConnectionString("DefaultConnection");
			services.AddDbContext<AppDbContext>(options =>
			{
				options.UseSqlServer(connectionString);
			});
			services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());

			return services;
		}
	}
}
