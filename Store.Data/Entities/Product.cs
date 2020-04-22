using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Store.Data.Entities
{
    /// <summary>
    /// 商品信息表
    /// </summary>
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; }
        public string ShortDescribe { get; set; }
        public decimal Price { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount{ get; set; }
        public string Pictrue { get; set; }
       
        public Guid ProductNo { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 库存量
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        public int PageView  { get; set; }

        /// <summary>
        /// 购买量
        /// </summary>
        public int Purchase { get; set; }

        public int Product_CategoryId { get; set; }
        public virtual Product_Category Product_Category { get; set; }
        public virtual ICollection<Product_Image> Product_Images { get; set; }
        public virtual ICollection<Product_Describe> Product_Describes { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
