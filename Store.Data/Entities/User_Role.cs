using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Store.Data.Entities
{
    /// <summary>
    /// 用户角色映射表
    /// </summary>
    public class User_Role
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
