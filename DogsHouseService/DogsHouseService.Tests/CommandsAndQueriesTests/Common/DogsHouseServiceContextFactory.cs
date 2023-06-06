using DogsHouseService.Domain;
using DogsHouseService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DogsHouseService.Tests.CommandsAndQueriesTests.Common
{
	public class DogsHouseServiceContextFactory
	{
		public static AppDbContext Create()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;
			var context = new AppDbContext(options);
			context.Database.EnsureCreated();
			context.Dogs.AddRange(
				new Dog
				{
					Name = "Name3",
					Color = "color3",
					TailLength = 3,
					Weight = 30
				},
				new Dog
				{
					Name = "Name4",
					Color = "color4",
					TailLength = 4,
					Weight = 40
				},
				new Dog
				{
					Name = "Name5",
					Color = "color5",
					TailLength = 5,
					Weight = 50
				}
			);
			
			context.SaveChanges();
			return context;
		}

		public static void Destroy(AppDbContext context)
		{
			context.Database.EnsureDeleted();
			context.Dispose();
		}
	}
}
