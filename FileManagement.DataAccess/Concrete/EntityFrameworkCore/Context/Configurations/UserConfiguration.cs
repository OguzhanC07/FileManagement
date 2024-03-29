﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using FileManagement.DataAccess.Concrete.EntityFrameworkCore.Context;
using FileManagement.DataAccess;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;


namespace FileManagement.DataAccess.Concrete.EntityFrameworkCore.Context.Configurations
{
    public partial class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.Property(e => e.Email).UseCollation("SQL_Latin1_General_CP1_CS_AS");

            entity.Property(e => e.Password).UseCollation("SQL_Latin1_General_CP1_CS_AS");

            entity.Property(e => e.Username).UseCollation("SQL_Latin1_General_CP1_CS_AS");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<User> entity);
    }
}
