using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRestCoreProducts.Data;
using ApiRestCoreProducts.Models;
using FluentValidation.Results;
using ApiRestCoreProducts.Validations;

namespace ApiRestCoreProducts.Controllers
{
    /// <summary>
    /// ProductsController
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApiRestCoreProductsContext _context;

        /// <summary>
        /// ProductsController
        /// </summary>
        /// <param name="context"></param>
        public ProductsController(ApiRestCoreProductsContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get a list of products
        /// </summary>
        /// <returns>List of products</returns>
        /// <response code="401">Unauthorized.</response>
        /// <response code="200">OK. Returns the requested object.</response>
        /// <response code="400">BadRequest.</response>
        /// <response code="404">NotFound. The requested object was not found.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return await _context.Product.ToListAsync();
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        /// <returns>Single product</returns>
        /// <param name="id">Product ID (GUID).</param>
        /// <response code="200">OK. Returns the requested object.</response>
        /// <response code="400">BadRequest.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="404">NotFound. The product was not found.</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        /// <summary>
        /// Update a single product
        /// </summary>
        /// <param name="id">Product ID (GUID).</param>
        /// <param name="product">Product to update.</param>
        /// <returns>No content response</returns>
        /// <response code="204">NoContent. Product updated. No content response.</response>
        /// <response code="400">BadRequest.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="404">NotFound. The product was not found.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            try
            {
                ProductValidator validator = new();
                ValidationResult result = validator.Validate(product);

                if (!result.IsValid)
                {
                    string msg = string.Empty;
                    foreach (var failure in result.Errors)
                    {
                        msg += @"Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage;
                        return BadRequest(msg);
                    }
                }

                _context.Entry(product).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Insert a single product
        /// </summary>
        /// <param name="product">Product to create.</param>
        /// <returns>The new created product</returns>
        /// <response code="200">OK. Returns the product created.</response>
        /// <response code="400">BadRequest.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="404">NotFound. The product was not found.</response>
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            try
            {
                ProductValidator validator = new();
                ValidationResult result = validator.Validate(product);

                if (!result.IsValid)
                {
                    string msg = string.Empty;
                    foreach (var failure in result.Errors)
                    {
                        msg += @"Property " + failure.PropertyName + " failed validation. Error was: " + failure.ErrorMessage;
                        return BadRequest(msg);
                    }
                }

                _context.Product.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetProduct", new { id = product.Id }, product);
            }
            catch (Exception)
            {
                return StatusCode(500, @"Unexpected error");
            }
        }

        /// <summary>
        /// Delete a product by id
        /// </summary>
        /// <param name="id">Product ID (GUID).</param>
        /// <returns>No content response</returns>
        /// <response code="204">NoContent. Product deleted. No content response.</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="404">NotFound. The product was not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Check if a product exists
        /// </summary>
        /// <param name="id">Product ID (GUID).</param>
        /// <returns>true if the product exists</returns>
        private bool ProductExists(Guid id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
