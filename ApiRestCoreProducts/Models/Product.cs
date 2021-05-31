using System;

namespace ApiRestCoreProducts.Models
{
	/// <summary>
	/// Product model
	/// </summary>
	public class Product
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public bool Available { get; set; }
	}
}
