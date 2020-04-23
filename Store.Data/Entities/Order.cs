using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Store.Data.Entities
{
    /// <summary>
    /// 订单表
    /// </summary>
    public class Order
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public DateTime Createtime { get; set; }

        public int Count { get; set; }

        public  decimal TotalPrices  { get; set; }

        public int Status { get; set; }

        public int RealPrice { get; set; }
    }
}
