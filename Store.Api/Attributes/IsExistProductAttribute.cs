using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Store.Api.Attributes
{
    public class IsExistProductAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var typeId= context.ActionArguments["id"].ToString();
            return base.OnActionExecutionAsync(context, next);
        }
     
    }
}
