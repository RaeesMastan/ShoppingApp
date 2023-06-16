using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Shopbridge_base.Data;
using Shopbridge_base.Data.Repository;
using Shopbridge_base.Domain.Models;
using System;
using System.Linq;

namespace Shopbridge_base.ActionFilters
{

    public class ValidateEntityExistsAttribute<T> : IActionFilter
    {       
        private readonly Shopbridge_Context _repository;
        public ValidateEntityExistsAttribute(Shopbridge_Context repository)
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
                var Params = context.RouteData.Values["id"];
                var ID = (Int16.Parse((string)Params));
                if (Params == null)
                {
                    context.Result = new BadRequestObjectResult("Id is null");
                    return;
                }
                if (!context.ModelState.IsValid)
                {
                    context.Result = new BadRequestObjectResult(context.ModelState);
                    return;
                }


                var id = _repository.Product.FirstOrDefault((a) => a.Product_Id == ID);
                if (id == null)
                {
                    context.Result = new JsonResult($" 404 Not Found product details not found with id {ID}");

                    return;
                }               
            }
            catch(Exception ex)
            {
                context.Result = new JsonResult(ex.Message);
                return;
            }
            

        }
    }

}
