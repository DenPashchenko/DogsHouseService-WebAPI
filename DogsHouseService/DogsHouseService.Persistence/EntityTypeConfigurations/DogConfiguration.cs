using DogsHouseService.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DogsHouseService.Persistence.EntityTypeConfigurations
{
	public class DogConfiguration : IEntityTypeConfiguration<Dog>
	{
		public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Dog> builder)
		{
			builder.HasKey(t => t.Id);

			builder.Property(t => t.Name)
				.IsRequired()
				.HasMaxLength(20)
				.HasColumnName("name");

			builder.Property(t => t.Color)
				.IsRequired()
				.HasMaxLength(30)
				.HasColumnName("color");

			builder.Property(t => t.TailLength)
				.HasColumnName("tail_length");

			builder.Property(t => t.Weight)
				.HasColumnName("weight");
		}
	}
}
