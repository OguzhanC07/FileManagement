using FileManagement.API.Models;
using FileManagement.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagement.API.CustomFilters
{
    public class ValidId<T> : IActionFilter where T : class, new()
    {
        private readonly IGenericService<T> _genericService;
        public ValidId(IGenericService<T> genericService)
        {
            _genericService = genericService;
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //context.ActionArguments
            //context.HttpContext.User.Identity.Name;

            var list = context.ActionArguments.ToList();

            var dictionary = context.ActionArguments.Where(I => I.Key == "id").FirstOrDefault();

            if (dictionary.Value == null)
            {
                context.Result = new NotFoundObjectResult(new ResponseMessageModel<string> { Result = false, Message = "The thing of you searched could not found found" });
            }
            else
            {
                var id = int.Parse(dictionary.Value.ToString());
                var entity = _genericService.GetById(id).Result;
                if (entity == null)
                {
                    context.Result = new NotFoundObjectResult(new ResponseMessageModel<T> { Result = false, Message = "The thing you searched could not found" });
                }
            }

        }
    }
}
