using IdentityServer4.EntityFramework.Stores;
using IdentityServer4.Stores;
using Is4Server.Services.Implementations;
using Is4Server.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Is4Server.Setups
{
    public class IocSetup
    {
        #region Methods

        /// <summary>
        ///     Run ioc setup.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void Run(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            //services.AddScoped<IProfileService, IdentityServerProfileService>();

            services.AddScoped<IBaseEncryptionService, EncryptionService>();

            // Persisted grant store registration.
            //services.AddTransient<IPersistedGrantStore, PersistedGrantStore>();
        }

        #endregion
    }
}