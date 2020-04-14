using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace Store.Core.Pages
{
    public class PageList<T>  
    {
        public PageList(List<T> item, int pageIndex, int pageSize, int totalCount)
        {
            Data= item;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            TotalCount = totalCount;
        }
        public PageList()
        {

        }

        public List<T> Data { get; set; }

         
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public static async Task<PageList<T>> CreateLayuiList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var totalCount = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            var list = new PageList<T>(items.ToList(), pageIndex, pageSize, totalCount);
            return await Task.FromResult(list);
        }
    }
}
