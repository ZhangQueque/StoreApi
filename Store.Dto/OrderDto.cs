﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Dto
{
    public class OrderDto
    {

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime CreateTime { get; set; }

        public int Count { get; set; }

        public decimal TotalPrices { get; set; }

        public int Status { get; set; }
        public string Size { get; set; }
        public string Title { get; set; }
        public string ShortDescribe { get; set; }
        public decimal Price { get; set; }
        public string Pictrue { get; set; }
    }
}
