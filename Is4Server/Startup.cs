using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using Is4Server.Setups;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Is4Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Run inversion of control.
            IocSetup.Run(services, Configuration);

            // Run authentication setup.
            AuthenticationSetup.Run(services, Configuration);
            
            // Add CORS support.
            CorsSetup.Run(services, Configuration);

            // Add http context accessor.
            services.AddHttpContextAccessor();

            // Add app db context.
            AppDbContextSetup.Run(services, Configuration);

            //// some details omitted
            //services.AddIdentityServer()
            //    .AddInMemoryApiResources(Is4Setup.LoadApiResources())
            //    .AddInMemoryIdentityResources(Is4Setup.LoadIdentityResources())
            //    .AddInMemoryClients(Is4Setup.LoadClients())
            //    .AddDeveloperSigningCredential();

            //services.AddAuthentication()
            //    .AddGoogle("Google", options =>
            //    {
            //        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            //        options.ClientId = "323676358406-ikvol20relacv3mn5popdi79e5m759pc.apps.googleusercontent.com";
            //        options.ClientSecret = "68pGK3guMhv_bdJKQOznblSi";
            //    })
            //    .AddOpenIdConnect("demoidsrv", "IdentityServer", options =>
            //    {
            //        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            //        options.SignOutScheme = IdentityServerConstants.SignoutScheme;

            //        options.Authority = "http://localhost:57547";
            //        options.ClientId = "implicit";
            //        options.ResponseType = "id_token";
            //        options.SaveTokens = true;
            //        options.CallbackPath = new PathString("/signin-idsrv");
            //        options.SignedOutCallbackPath = new PathString("/signout-callback-idsrv");
            //        options.RemoteSignOutPath = new PathString("/signout-idsrv");
            //        options.RequireHttpsMetadata = false;

            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            NameClaimType = "name",
            //            RoleClaimType = "role"
            //        };
            //    });

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseIdentityServer();

            //AuthenticationSetup.Seed(app);

            app
                .UseMvc(options =>
                {
                    options.MapRoute(
                        "default",
                        "{controller=Main}/{action=Index}/{id?}");
                });
        }
    }
}