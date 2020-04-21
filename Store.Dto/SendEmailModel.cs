using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Dto
{
    /// <summary>
    /// 邮件发送帮助类
    /// </summary>
    public class SendEmailModel
    {
        
        public string Title { get; set; }
       
        public string Content { get; set; }
       
        public string EmailAddress { get; set; }
       
        public string Addresser { get; set; }

        public string Recipients { get; set; }
      
        public string Key { get; set; }
    }
}
