using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
namespace Store.Data.Entities
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserInfo
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Password { get; set; }
        public int IsExistPhone { get; set; }

        public int IsExistEmail { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateId { get; set; }

        public DateTime UpdateTime { get; set; }

        public string ShippingAddress { get; set; }
 
    }
}
