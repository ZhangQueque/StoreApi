using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Dto
{
    public class ProductCreateDto
    {
        public string Title { get; set; }
        public string ShortDescribe { get; set; }
        public decimal Price { get; set; }
        public IFormFile Pictrue { get; set; }
        public int Stock { get; set; }
        public int Product_CategoryId { get; set; }

    }
}
