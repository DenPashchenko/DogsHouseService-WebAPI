using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DogsHouseService.Domain
{
	public class Dog
	{
		public int Id { get; set; }

		public string Name { get; set; } = null!;

		public string Color { get; set; } = null!;

		public int TailLength { get; set; }

		public int Weight { get; set; }
	}
}
