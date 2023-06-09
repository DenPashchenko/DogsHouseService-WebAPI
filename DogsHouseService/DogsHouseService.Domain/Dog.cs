﻿namespace DogsHouseService.Domain
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
