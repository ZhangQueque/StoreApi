using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Dto
{
    public class UpdatePasswordDto
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
