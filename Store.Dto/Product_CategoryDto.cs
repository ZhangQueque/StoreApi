using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Dto
{
    public class Product_CategoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int PId { get; set; }

        public int SortId { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public List<Product_CategoryDto> ChildList { get; set; } = new List<Product_CategoryDto>();
     
    }
}
