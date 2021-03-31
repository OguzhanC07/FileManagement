using FileManagement.Business.DTOs.UserDto;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace FileManagement.Business.FluentValidation
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(I => I.UserName).Length(1,30).WithMessage(localizer["MaxLengthValidator"].ToString().Replace("{0}",localizer["Username"]).Replace("{1}","{MaxLength}").Replace("{2}", "{MinLength}"));
            RuleFor(I => I.Password).Length(1, 30).WithMessage(localizer["MaxLengthValidator"].ToString().Replace("{0}",localizer["Password"]).Replace("{1}", "{MaxLength}").Replace("{2}", "{MinLength}"));
            RuleFor(I => I.UserName).NotEmpty().WithMessage(localizer["EmptyValidator"].ToString().Replace("{0}", localizer["Username"]));
            RuleFor(I => I.Password).NotEmpty().WithMessage(localizer["EmptyValidator"].ToString().Replace("{0}", localizer["Password"]));
        }
    }
}
