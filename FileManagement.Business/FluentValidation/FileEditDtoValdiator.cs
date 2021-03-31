using FileManagement.Business.DTOs.FileDto;
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
    public class FileEditDtoValdiator : AbstractValidator<FileEditDto>
    {
        public FileEditDtoValdiator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(I => I.FileName).NotEmpty().WithMessage(localizer["EmptyValidator"].ToString().Replace("{0}",localizer["FileName"]));
            RuleFor(I => I.FileName).Matches("^[A-Za-z0-9' -]+$").WithMessage(localizer["ValidNameValidator"].ToString().Replace("{0}",localizer["FileName"]));
            RuleFor(I => I.FileName).Length(3, 50).WithMessage(localizer["MaxLengthValidator"].ToString().Replace("{0}", localizer["FileName"]).Replace("{1}","{MaxLength}").Replace("{2}", "{MinLength}"));
        }
    }
}
