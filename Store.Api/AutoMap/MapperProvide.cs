using AutoMapper;
using Store.Data.Entities;
using Store.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Api.AutoMap
{
    public class MapperProvide:Profile
    {
        public MapperProvide()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
