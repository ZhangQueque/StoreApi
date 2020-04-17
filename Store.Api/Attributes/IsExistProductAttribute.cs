using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Service;

namespace Store.Api.Attributes
{
    public class IsExistProductAttribute : ActionFilterAttribute
    {
        private readonly IRepositoryWrapper repositoryWrapper;

        public IsExistProductAttribute(IRepositoryWrapper repositoryWrapper)
        {
            this.repositoryWrapper = repositoryWrapper;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           
            var typeId= context.ActionArguments["typeId"];
            
            if ((int)typeId!=0)
            {
                if (!await repositoryWrapper.Product_CategoryRepository.IsExistProductsAsync((int)typeId))
                {
                    context.Result = new NotFoundResult();

                }
            }
            
            await  base.OnActionExecutionAsync(context, next);
        }
     
    }
}
