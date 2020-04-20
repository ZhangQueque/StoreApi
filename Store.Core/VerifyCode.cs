using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core
{
    /// <summary>
    /// 生成验证码
    /// </summary>
    public class VerifyCode
    {
        public VerifyCode()
        {
            random = new Random();
        }

        Random random;//生成随机数对象
        string VCode;//验证码字符串
        /// <summary>
        /// 创建验证码字符串
        /// </summary>
        /// <param name="codeLen">验证码长度</param>
        public string CreateVCode(int codeLen = 4)
        {
            //获取随机数字
            VCode = null;
            for (int i = 0; i < codeLen; i++)
            {
                VCode += random.Next(0, 10);
            }

            return VCode;
        }
    }
}
