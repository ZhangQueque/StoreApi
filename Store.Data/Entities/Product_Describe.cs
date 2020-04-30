using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Store.Data.Entities
{
    /// <summary>
    /// 商品描述表
    /// </summary>
    public class Product_Describe
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }

        public int ProductId { get; set; }

     }
}
