using FileManagement.Business.DTOs.UserDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.Business.FluentValidation
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(I => I.UserName).NotEmpty().WithMessage("Username can't be empty");
            RuleFor(I => I.Password).NotEmpty().WithMessage("Password can't be empty");
        }
    }
}
