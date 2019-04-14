using System;
using Is4ServerWithoutUi.Enums;

namespace Is4ServerWithoutUi.Models.Entities
{
    public class User
    {
        #region Properties

        /// <summary>
        /// Id of user.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Username of account.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Password of user account.
        /// </summary>
        public string HashedPassword { get; set; }

        /// <summary>
        /// Email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User birthday.
        /// </summary>
        public double Birthday { get; set; }

        /// <summary>
        /// Kind of user account.
        /// </summary>
        public UserKinds Kind { get; set; }

        /// <summary>
        /// User status.
        /// </summary>
        public UserStatuses Status { get; set; }

        /// <summary>
        /// Time when user joined in the system.
        /// </summary>
        public double JoinedTime { get; set; }

        /// <summary>
        /// Time when user account was modified.
        /// </summary>
        public double? LastModifiedTime { get; set; }

        #endregion

        #region Constructor

        public User(Guid id, string username)
        {
            Id = id;
            Username = username;
        }

        #endregion
    }
}