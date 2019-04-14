using Is4ServerWithoutUi.Models.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Is4ServerWithoutUi.Setups
{
    public class AppDbContextSetup
    {
        #region Methods

        /// <summary>
        /// Run db context.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void Run(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LearnIs4DbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("LearnIs4")));

            services.AddScoped<DbContext, LearnIs4DbContext>();
        }

        #endregion
    }
}