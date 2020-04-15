using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;
using Store.Data.Entities;

namespace Store.Dto
{
    [ProtoContract]
    public class Product_CategoryDto
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public string Title { get; set; }

        [ProtoMember(3)]
        public int PId { get; set; }

        [ProtoMember(4)]
        public int SortId { get; set; }

        [ProtoMember(5)]
        public int Status { get; set; }

        [ProtoMember(6)]
        public DateTime CreateTime { get; set; }

        [ProtoMember(7)]
        public List<Product_CategoryDto> ChildList { get; set; } = new List<Product_CategoryDto>();
        [ProtoMember(8)]
        public List<Product> ProductList { get; set; } = new List<Product>();

    }
}
