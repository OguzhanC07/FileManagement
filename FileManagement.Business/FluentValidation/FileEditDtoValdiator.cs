using FileManagement.Business.DTOs.FileDto;
using FluentValidation;
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
        public FileEditDtoValdiator()
        {
            RuleFor(I => I.FileName).NotEmpty().WithMessage("File name can't be empty");
            RuleFor(I => I.FileName).Matches("^[A-Za-z0-9' -]+$", RegexOptions.CultureInvariant).WithMessage("Folder name must be alphanumeric characters");
            Regex regex = new Regex(@"^[\w\-. ]+$", RegexOptions.IgnoreCase);
            RuleFor(I => I.FileName).Matches(regex).WithMessage("File name is not correct");
        }
    }
}
