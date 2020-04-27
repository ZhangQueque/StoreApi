using Store.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Password { get; set; }
        public int IsExistPhone { get; set; }

        public int IsExistEmail { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public string Image { get; set; }

        public int CreateId { get; set; }

        public DateTime UpdateTime { get; set; }

        public string ShippingAddress { get; set; }

        public User_Role User_Role { get; set; }
    }
}
