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
					Name = "Name1",
					Color = "Color1",
					TailLength = 1,
					Weight = 10
				},
				new Dog
				{
					Name = "Name2",
					Color = "Color2",
					TailLength = 2,
					Weight = 20
				},
				new Dog
				{
					Name = "Name3",
					Color = "Color3",
					TailLength = 3,
					Weight = 30
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
