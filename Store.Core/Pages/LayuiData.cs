using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Core.Pages
{
    public class LayuiData
    {
        public int Code { get; set; } = 0;
        public int Count { get; set; }
        public dynamic Data { get; set; }
        public string Msg { get; set; } = "";
    }
}
