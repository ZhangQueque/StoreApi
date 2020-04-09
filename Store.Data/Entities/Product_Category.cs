using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using System.ComponentModel;

namespace Store.Data.Entities
{
    /// <summary>
    /// 商品分类表
    /// </summary>
    public class Product_Category
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(20)]
        public string Title { get; set; }

        public int PId { get; set; }

        public int SortId { get; set; }

       
        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Product_Size> Product_Sizes { get; set; }
    }
}
