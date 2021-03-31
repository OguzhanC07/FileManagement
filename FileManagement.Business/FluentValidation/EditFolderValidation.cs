﻿using FileManagement.Business.DTOs.FolderDto;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileManagement.Business.FluentValidation
{
    public class EditFolderValidation : AbstractValidator<FolderEditDto>
    {
        public EditFolderValidation(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(I => I.FolderName).NotEmpty().WithMessage(localizer["EmptyValidator"].ToString().Replace("{0}", localizer["FolderName"]));
            RuleFor(I => I.FolderName).Matches("^[A-Za-z0-9' -]+$").WithMessage(localizer["ValidNameValidator"].ToString().Replace("{0}", localizer["FolderName"]));
            RuleFor(I => I.FolderName).Length(3, 50).WithMessage(localizer["MaxLengthValidator"].ToString().Replace("{0}", localizer["FolderName"]).Replace("{1}", "{MaxLength}").Replace("{2}","{MinLength}"));
        }
    }
}
