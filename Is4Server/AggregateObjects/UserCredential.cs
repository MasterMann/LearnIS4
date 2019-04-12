using System;
using System.Collections.Generic;
using System.Security.Claims;
using Is4Server.Models.Entities;

namespace Is4Server.AggregateObjects
{
    public class UserCredential
    {
        #region Constructor

        public UserCredential(User user)
        {
            _user = user;
        }

        #endregion

        #region Properties

        /// <summary>
        /// User information from database.
        /// </summary>
        private readonly User _user;

        #endregion

        #region Methods

        /// <summary>
        /// Get list of available claims that current user can have.
        /// </summary>
        public IList<Claim> GetClaims()
        {
            if (_user == null)
                throw new Exception("No user information has been attached into this identity credential");

            var claims = new List<Claim>();
            claims.Add(new Claim("id", _user.Id.ToString("D"), ClaimValueTypes.String));
            claims.Add(new Claim("username", _user.Username, ClaimValueTypes.String));
            claims.Add(new Claim("birthday", $"{_user.Birthday}", ClaimValueTypes.Double));
            claims.Add(new Claim("email", $"{_user.Email}", ClaimValueTypes.String));
            claims.Add(new Claim("joinedTime", $"{_user.JoinedTime}", ClaimValueTypes.Double));
            claims.Add(new Claim("lastModifiedtime", $"{_user.LastModifiedTime}", ClaimValueTypes.Double));
            return claims;
        }

        /// <summary>
        /// Get user entity from credential.
        /// </summary>
        /// <returns></returns>
        public User ToUser()
        {
            return _user;
        }

        #endregion

    }
}