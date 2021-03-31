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
        private readonly IStringLocalizer<SharedResource> _localizer;
        public ValidId(IGenericService<T> genericService, IStringLocalizer<SharedResource> localizer)
        {
            _genericService = genericService;
            _localizer = localizer;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var list = context.ActionArguments.ToList();

            var dictionary = context.ActionArguments.Where(I => I.Key == "id").FirstOrDefault();
            
            string type = _localizer["NotFoundError"];
            if (dictionary.Value == null)
            {
                context.Result = new NotFoundObjectResult(new ResponseMessageModel<string> { Result = false, Message =type.Replace("{0}",_localizer[typeof(T).Name]) });
            }
            else
            {
                var id = int.Parse(dictionary.Value.ToString());
                var entity = _genericService.GetById(id).Result;
                if (entity == null)
                {
                    context.Result = new NotFoundObjectResult(new ResponseMessageModel<string> { Result = false, Message = type.Replace("{0}", _localizer[typeof(T).Name]) });
                }
            }

        }
    }
}
