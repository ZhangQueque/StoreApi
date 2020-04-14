using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Dto
{
    /// <summary>
    /// 商品Dto
    /// </summary>
    public class ProductDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string ShortDescribe { get; set; }
        public decimal Price { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }
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
        public int PageView { get; set; }

        /// <summary>
        /// 购买量
        /// </summary>
        public int Purchase { get; set; }
    }
}
