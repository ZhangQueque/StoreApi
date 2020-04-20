using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Dto
{
    /// <summary>
    /// 短信发送帮助类
    /// </summary>
    public class SendPhoneModel
    {
        public string Phone { get; set; }

        public string Code { get; set; }

        public string Key { get; set; }
    }
}
