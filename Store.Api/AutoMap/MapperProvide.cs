using AutoMapper;
using Store.Data.Entities;
using Store.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Api.AutoMap
{
    public class MapperProvide : Profile
    {
        public MapperProvide()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<UserInfo, User_EmailLoginDto>();
            CreateMap<User_EmailLoginDto, UserInfo>();

            CreateMap<UserInfo, User_PhoneLoginDto>();
            CreateMap<User_PhoneLoginDto, UserInfo>();
            CreateMap<UserInfo, UserDto>();
         }
    }
}
