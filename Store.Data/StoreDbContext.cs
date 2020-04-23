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

        //用户角色
        public DbSet<UserInfo> UserInfos { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<User_Role> User_Roles { get; set; }

        //商品
        public DbSet<Product_Category> product_Categories { get; set; }
      
        public DbSet<Product> Products { get; set; }
        public DbSet<Product_Image> Product_Images { get; set; }
        public DbSet<Product_Size> Product_Sizes { get; set; }
        public DbSet<Product_Describe> Product_Describes { get; set; }
        public DbSet<Wish> Wishes { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<Order> Orders { get; set; }
    }
}
