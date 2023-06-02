using DogsHouseService.Application.Interfaces;
using DogsHouseService.Domain;
using DogsHouseService.Persistence.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogsHouseService.Persistence
{
	public class AppDbContext : DbContext, IAppDbContext
	{
		public DbSet<Dog> Dogs { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfiguration(new DogConfiguration());

			builder.Entity<Dog>().HasData(
				new Dog { Id = 1, Name = "Neo", Color = "red&amber", TailLength = 22, Weight = 32 },
				new Dog { Id = 2, Name = "Jessy", Color = "black&white", TailLength = 7, Weight = 14 }
				);

			base.OnModelCreating(builder);
		}
	}
}
