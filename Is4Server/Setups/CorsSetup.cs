using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Is4Server.Setups
{
    public class CorsSetup
    {
        #region Methods

        /// <summary>
        /// Run cors setup.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void Run(IServiceCollection services, IConfiguration configuration)
        {
            // Cors configuration.
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.WithExposedHeaders("WWW-Authenticate");
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();

            // Add cors configuration to service configuration.
            services.AddCors(options => { options.AddPolicy("AllowAll", corsBuilder.Build()); });

        }

        #endregion
    }
}