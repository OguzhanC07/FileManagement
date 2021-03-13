using FileManagement.Business.DTOs.FolderDto;
using FluentValidation;
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
        public EditFolderValidation()
        {
            RuleFor(I => I.FolderName).NotEmpty().WithMessage("Folder name can't be empty");
            RuleFor(I => I.FolderName).Matches("^[A-Za-z0-9' -]+$").WithMessage("Folder name must be alphanumeric characters");
        }
    }
}
