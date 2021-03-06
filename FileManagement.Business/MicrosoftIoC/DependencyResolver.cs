using FileManagement.Business.Concrete;
using FileManagement.Business.DTOs.FileDto;
using FileManagement.Business.DTOs.FolderDto;
using FileManagement.Business.DTOs.UserDto;
using FileManagement.Business.FluentValidation;
using FileManagement.Business.Interfaces;
using FileManagement.Business.JwtTool;
using FileManagement.DataAccess.Concrete.EntityFrameworkCore.Repositories;
using FileManagement.DataAccess.Interfaces;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.Business.MicrosoftIoC
{
    public static class DependencyResolver
    {
        public static void AddDependicies(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericService<>), typeof(GenericManager<>));
            services.AddScoped(typeof(IGenericDal<>), typeof(GenericRepository<>));


            services.AddScoped<IFileDal, FileRepository>();
            services.AddScoped<IFolderDal, FolderRepository>();
            services.AddScoped<IUserDal, UserRepository>();

            services.AddScoped<IFileService, FileManager>();
            services.AddScoped<IFolderService, FolderManager>();
            services.AddScoped<IUserService, UserManager>();


            services.AddScoped<IJwtService, JwtManager>();

            services.AddTransient<IValidator<UserLoginDto>, UserLoginDtoValidator>();
            services.AddTransient<IValidator<AddFolderDto>, AddFolderDtoValidation>();
            services.AddTransient<IValidator<FolderEditDto>, EditFolderValidation>();
            services.AddTransient<IValidator<FileEditDto>, FileEditDtoValdiator>();
        }
    }
}
