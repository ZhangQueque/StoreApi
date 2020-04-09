using Microsoft.EntityFrameworkCore;
using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Data
{
    public class StoreDbContext :DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options):base (options)
        {

        }

        public DbSet<Product_Category> product_Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Product_Image> Product_Images { get; set; }
        public DbSet<Product_Size> Product_Sizes { get; set; }
        public DbSet<Product_Describe> Product_Describes { get; set; }
    }
}
