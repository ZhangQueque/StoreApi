using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Pages
{
    public class PageMessage
    {
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
