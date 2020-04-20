using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Store.Dto
{
    public class User_PhoneLoginDto
    {
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
