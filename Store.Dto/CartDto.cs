using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Dto
{
    /// <summary>
    /// 购物车Dto
    /// </summary>
    public class CartDto
    {
 
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateTime { get; set; }
        public string Size { get; set; }
        public int Count { get; set; }
        public string Title { get; set; }
        public string ShortDescribe { get; set; }
        public decimal Price { get; set; }
        public string Pictrue { get; set; }
    }
}
