using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Dto
{
    /// <summary>
    /// 收藏Dto模型
    /// </summary>
    public class WishDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Createtime { get; set; }
        public string Title { get; set; }
        public string ShortDescribe { get; set; }
        public decimal Price { get; set; }
        public string Pictrue { get; set; }

    }
}
