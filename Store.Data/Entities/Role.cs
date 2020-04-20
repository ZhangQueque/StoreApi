using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Store.Data.Entities
{
    /// <summary>
    /// 角色
    /// </summary>
    public class Role
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public int CreateId { get; set; }
    }
}
