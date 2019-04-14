using Is4ServerWithoutUi.Services.Implementations;
using Is4ServerWithoutUi.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Is4ServerWithoutUi.Setups
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