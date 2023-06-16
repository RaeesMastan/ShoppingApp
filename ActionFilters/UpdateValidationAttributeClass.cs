using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shopbridge_base.Data;
using Shopbridge_base.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopbridge_base.ActionFilters
{
    public class UpdateValidationAttributeClass<T> : IActionFilter
    { 
        private readonly Shopbridge_Context _repository;
        public UpdateValidationAttributeClass(Shopbridge_Context repository)
        {
            _repository = repository;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public async void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                Product body = (Product)context.ActionArguments["_product"];
                int Id = (int)context.ActionArguments["id"];


                if (Id == null)
                {
                    new JsonResult(" 404 Id is null");
                    return;
                }

                if (body == null)
                {
                    if (body == null)
                    {
                        new JsonResult("Product is null");
                        return;
                    }


                }

            }
            catch (Exception ex)
            {
                context.Result = new JsonResult(ex.Message);
                return;
            }


        }
    }
}
