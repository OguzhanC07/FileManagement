using AutoMapper;
using FileManagement.Business.DTOs.FileDto;
using FileManagement.Business.DTOs.FolderDto;
using FileManagement.Business.DTOs.UserDto;
using FileManagement.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileManagement.API.DtoMapping
{
    public class DtoMaps : Profile
    {
        public DtoMaps()
        {
            #region User
            CreateMap<User, UserLoginDto>();
            CreateMap<UserLoginDto, User>();
            #endregion

            #region Folder
            CreateMap<Folder, AddFolderDto>();
            CreateMap<AddFolderDto, Folder>();

            CreateMap<Folder, FolderListDto>();
            CreateMap<FolderListDto, Folder>();

            CreateMap<Folder, FolderEditDto>();
            CreateMap<FolderEditDto, Folder>();
            #endregion

            #region File
            CreateMap<File, FileEditDto>();
            CreateMap<FileEditDto, File>();
            #endregion
        }
    }
}
