using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Dto
{
    public class UserUpdateDto
    {
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public IFormFile Image { get; set; }
        public string ShippingAddress { get; set; }

    }
}
