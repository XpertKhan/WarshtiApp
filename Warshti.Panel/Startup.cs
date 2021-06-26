using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities;
using Warshti.Panel.Data;
using Warshti.Panel.Profiles;
using Warshti.Repositories;
using WScore.Entities.Identity;

namespace Warshti.Panel
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder().SetBasePath(_env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true);
            _config = builder.Build();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null); ;
            services.AddSingleton(_config);

            services.AddTransient<DataSeeder>();
            services.AddDbContext<WScoreContext>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WScoreContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IEFRepository, EFRepository>();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Auth/LogIn";
                config.LogoutPath = "/Auth/LogOut";
                config.SlidingExpiration = true;

                config.AccessDeniedPath = "/Auth/AccessDenied";
                config.ReturnUrlParameter = "returnUrl";
            });

            services.Configure<IdentityOptions>(config =>
            {
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequiredLength = 8;
                config.Password.RequiredUniqueChars = 0;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;

                config.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
                config.User.RequireUniqueEmail = true;
            });

            services.AddAutoMapper(config =>
            {
                config.AddProfile(new UserProfile());
                config.AddProfile(new WorkShopProfile());
                config.AddProfile(new WorkShopInfoProfile());
                config.AddProfile(new WorkShopImageProfile());
                config.AddProfile(new RoleProfile());
                config.AddProfile(new AnnouncementProfile());
                config.AddProfile(new NotificationProfile());
                config.AddProfile(new ServiceProfile());
                config.AddProfile(new OrderProfile());
                config.AddProfile(new OrderStepProfile());
            }, typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            // Add cookie-based authentication to the request pipeline
            app.UseAuthentication();

            // Add the authorization middleware to the request pipeline
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
