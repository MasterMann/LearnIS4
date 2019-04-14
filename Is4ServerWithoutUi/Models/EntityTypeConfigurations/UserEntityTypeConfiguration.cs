using System;
using System.Collections.Generic;
using Is4ServerWithoutUi.Enums;
using Is4ServerWithoutUi.Models.Entities;
using Is4ServerWithoutUi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Is4ServerWithoutUi.Models.EntityTypeConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        #region Properties

        private readonly IBaseEncryptionService _encryptionService;

        #endregion

        #region Constructor

        public UserEntityTypeConfiguration(IBaseEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        #endregion

        #region Constructor

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Username).IsRequired();
            builder.Property(x => x.Email).IsRequired();

            var users = new List<User>();
            var linhNguyen = new User(Guid.NewGuid(), "linh.nguyen");
            linhNguyen.HashedPassword = _encryptionService.Md5Hash("administrator");
            linhNguyen.Kind = UserKinds.Basic;
            linhNguyen.Status = UserStatuses.Active;
            users.Add(linhNguyen);

            var alakanzam = new User(Guid.NewGuid(), "lightalakanzam@gmail.com");
            alakanzam.Kind = UserKinds.Google;
            alakanzam.Status = UserStatuses.Active;
            users.Add(alakanzam);

            builder.HasData(users.ToArray());

        }

        #endregion
    }
}