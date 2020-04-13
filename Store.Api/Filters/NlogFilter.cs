using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Api.Filter
{
    public class NlogFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<Program> logger;

        public NlogFilter(IWebHostEnvironment env ,ILogger<Program> logger)
        {
            this.env = env;
            this.logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            ErrorClass error = new ErrorClass();
            error.Message = context.Exception.Message;
            error.Detail = context.Exception.ToString();
            //context.Result = new ObjectResult(error)
            //{
            //    StatusCode = StatusCodes.Status500InternalServerError
            //};
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"服务器异常：{context.Exception.Message}");
            stringBuilder.Append($"具体详情：{context.Exception.ToString()}");

            logger.LogCritical(stringBuilder.ToString());
        }
    }
}
