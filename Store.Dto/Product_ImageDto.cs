using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Dto
{
    public class Product_ImageDto
    {

        public IFormFile Image1 { get; set; }

        public IFormFile Image2 { get; set; }

    }
}
