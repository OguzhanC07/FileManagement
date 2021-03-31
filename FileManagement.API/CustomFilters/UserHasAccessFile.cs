using FileManagement.API.Controllers;
using FileManagement.API.Models;
using FileManagement.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Security.Claims;

namespace FileManagement.API.CustomFilters
{
    public class UserHasAccessFile : ActionFilterAttribute
    {
        public bool HaveFolderId { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var thisController = ((FileController)context.Controller);
            IFolderService sr = thisController._folderService;
            IFileService fs = thisController._fileService;
            IStringLocalizer<SharedResource> localizer = thisController._localizer;
            int userId = Convert.ToInt32(context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var dictionary = context.ActionArguments.Where(I => I.Key == "id").FirstOrDefault().Value;

            if (HaveFolderId == true)
            {
                if (dictionary != null)
                {
                    var folder = sr.FindFolderById(Convert.ToInt32(dictionary)).Result;
                    if (folder != null && folder.AppUserId != userId)
                    {
                        context.Result = new UnauthorizedObjectResult(new ResponseMessageModel<string> { Result = false, Message = "You don't have access for this folder." });
                    }
                }
            }
            else
            {
                var file = fs.GetById(Convert.ToInt32(dictionary)).Result;
                var folder = sr.FindFolderById(file.FolderId).Result;
                
                if (file != null && folder != null && folder.AppUserId != userId)
                {
                    context.Result = new UnauthorizedObjectResult(new ResponseMessageModel<string>
                    {
                        Result = false,
                        Message = localizer["DontHaveAccessFile"]
                    });
                }
            }
        }
    }
}
