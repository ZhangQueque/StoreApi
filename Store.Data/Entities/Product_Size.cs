using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Store.Data.Entities
{
    /// <summary>
    /// 类别尺码表
    /// </summary>
    public class Product_Size
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Value { get; set; }

        public int Product_CategoryId { get; set; }
        public virtual Product_Category Product_Category { get; set; }
    }
}
