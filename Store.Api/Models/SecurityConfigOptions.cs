using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Api.Models
{
    /// <summary>
    /// jwt 配置文件
    /// </summary>
    public class SecurityConfigOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
}
