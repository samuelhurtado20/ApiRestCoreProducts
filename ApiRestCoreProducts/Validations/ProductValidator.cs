using ApiRestCoreProducts.Models;
using FluentValidation;

namespace ApiRestCoreProducts.Validations
{
	/// <summary>
	/// 
	/// </summary>
	public class ProductValidator : AbstractValidator<Product>
	{
		/// <summary>
		/// Validation rules for: Product
		/// </summary>
		public ProductValidator()
		{
			RuleFor(product => product.Name).NotNull().NotEmpty().WithMessage("Please specify a name for the product");
			RuleFor(product => product.Description).NotNull().NotEmpty().WithMessage("Please specify a description for the product");
			RuleFor(x => x.Price).GreaterThan(0).WithMessage("Please specify a price greater than 0");
			RuleFor(x => x.Available).NotNull().WithMessage("You must specify if the product is available");
		}
	}
}
