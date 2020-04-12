using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Pages
{
    /// <summary>
    /// 请求参数
    /// </summary>
    public class PageParameters
    {
        public int PageIndex { get; set; } = 1;

        private int pageSize = 32;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > 50) ? 50 : value; }
        }
        public string Name { get; set; } = "";

        public decimal BottomPrice { get; set; }

        public decimal TopPrice { get; set; }


        /// <summary>
        /// true 升序，false 降序
        /// </summary>
        public bool? IsPriceSort { get; set; }

        public bool? IsPurchaseSort { get; set; }

        public bool? IsTimeSort { get; set; }
    }
}
