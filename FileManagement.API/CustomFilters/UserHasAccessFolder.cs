using FileManagement.API.Controllers;
using FileManagement.API.Models;
using FileManagement.Business.Interfaces;
using FileManagement.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FileManagement.API.CustomFilters
{
    public class UserHasAccessFolder : ActionFilterAttribute
    {
        public bool CheckUserId { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var thisController = ((FolderController)context.Controller);

            IFolderService sr = thisController._folderService;

            int userId = Convert.ToInt32(context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var dictionary = context.ActionArguments.Where(I => I.Key == "id").FirstOrDefault().Value;

            if (CheckUserId==true)
            {
                if (dictionary != null)
                {
                    if (Convert.ToInt32(dictionary) != userId)
                    {
                        context.Result = new UnauthorizedObjectResult(new SingleResponseMessageModel<string> { Result = false, Message = "You don't have access for this folders" });
                    }
                }
            }
            else
            {
                var entity = sr.FindFolderById(Convert.ToInt32(dictionary)).Result;
                if (entity!=null && entity.AppUserId != userId)
                {
                    context.Result = new UnauthorizedObjectResult(new SingleResponseMessageModel<string>
                    {
                        Result = false,
                        Message = "You don't have access for this folder"
                    });
                }
            }
        }
    }
}
