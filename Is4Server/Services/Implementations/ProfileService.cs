using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Is4Server.AggregateObjects;
using Is4Server.Models.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Is4Server.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        #region Properties

        private readonly LearnIs4DbContext _dbContext;

        private readonly HttpContext _httpContext;

        #endregion

        #region Constructor

        public ProfileService(DbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = (LearnIs4DbContext)dbContext;
            _httpContext = httpContextAccessor.HttpContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // Get user which is attached to http context.
            var userCredential = (UserCredential)_httpContext.Items[ClaimTypes.UserData];

            if (userCredential == null)
                throw new Exception("No user credential has been found in the incoming http request.");

            context.IssuedClaims = context.Subject.Claims.ToList();
            return Task.CompletedTask;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual async Task IsActiveAsync(IsActiveContext context)
        {
            // Get subject from context (set in ResourceOwnerPasswordValidator.ValidateAsync),
            var username = context.Subject.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(username))
                return;

            var userCredential = (UserCredential)_httpContext.Items[ClaimTypes.UserData];
            if (userCredential == null)
            {
                var user = await _dbContext.Users
                    .Where(x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefaultAsync();

                _httpContext.Items[ClaimTypes.UserData] = new UserCredential(user);
            }

            context.IsActive = true;
        }

        #endregion
    }
}