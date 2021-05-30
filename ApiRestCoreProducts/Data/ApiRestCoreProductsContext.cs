using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApiRestCoreProducts.Models;

namespace ApiRestCoreProducts.Data
{
    public class ApiRestCoreProductsContext : DbContext
    {
        public ApiRestCoreProductsContext (DbContextOptions<ApiRestCoreProductsContext> options)
            : base(options)
        {
        }

        public DbSet<ApiRestCoreProducts.Models.Product> Product { get; set; }
    }
}
