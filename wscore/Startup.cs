using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Swashbuckle;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using WScore.Entities.Identity;
using AutoMapper;
using WScore.Profiles;
using WScore.Helpers;
using AutoWrapper;
using Microsoft.OpenApi.Models;
using Warshti.Entities;
using Warshti.Repositories;
using Newtonsoft.Json;

namespace WScore
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


        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddDbContext<WScoreContext>();
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WScoreContext>()
                .AddDefaultTokenProviders();
            services.AddTransient<AccountHelper>();
            services.AddTransient<DistanceHelper>();

            services.AddTransient<IEFRepository, EFRepository>();

            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            });

            //services.AddAuthentication(auth =>
            //{
            //    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options =>
            //{
            //    options.SaveToken = true;
            //    options.RequireHttpsMetadata = false;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidAudience = _config["Tokens:Audience"],
            //        ValidIssuer = _config["Tokens:Issuer"],
            //        RequireExpirationTime = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"])),
            //        ValidateIssuerSigningKey = true
            //    };
            //});

            services.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WARCHTI Service",
                    Version = "v1",
                    Description = "WARCHTI Service to share the api structure with frontend developers",
                    Contact = new OpenApiContact
                    {
                        Name = "Excelorithm",
                        Email = "hasnain@excelorithm.com"
                    },
                    TermsOfService = new Uri("https://xd.adobe.com/view/61fb9e5e-c892-4710-847f-d7ae6c89ee20-34a8/grid")
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });

            services.AddCors();
            //services.AddCors(options =>
            //{

            //    options.AddPolicy("CorsPolicy",
            //        builder => builder
            //        .SetIsOriginAllowed((host) => true)
            //        .AllowAnyMethod()
            //        .AllowAnyHeader()
            //        .AllowCredentials());
            //});

            services.AddAutoMapper(config =>
            {
                config.AddProfile(new UserProfile());
                config.AddProfile(new AnnouncementProfile());
                config.AddProfile(new AnnouncementImageProfile());
                config.AddProfile(new ColorProfile());
                config.AddProfile(new CompanyProfile());
                config.AddProfile(new ModelProfile());
                config.AddProfile(new TransmissionProfile());
                config.AddProfile(new PaymentMethodProfile());
                config.AddProfile(new DepartmentProfile());
                config.AddProfile(new FaultProfile());
                config.AddProfile(new ServiceProfile());
                config.AddProfile(new ServiceFaultProfile());
                config.AddProfile(new OrderProfile());
                config.AddProfile(new OrderStepProfile());
                config.AddProfile(new ChatProfile());
                config.AddProfile(new LanguageProfile());

                config.AddProfile(new WorkShopInfoProfile());
                config.AddProfile(new UserPaymentMethodProfile());
                config.AddProfile(new NotificationProfile());
                config.AddProfile(new UserSettingProfile());
                config.AddProfile(new FaqProfile());
                config.AddProfile(new QuestionProfile());
            }, typeof(Startup));
             
            services.Configure<SmtpSenderOptions>(_config);
            services.AddTransient<IEmailService, EmailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, WScoreContext wScoreContext)
        {
            wScoreContext.Database.Migrate();
            if (_env.IsDevelopment())
            {
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseApiResponseAndExceptionWrapper();


            app.UseRouting();

            // app.UseCors("CorsPolicy");
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());
            //app.UseAuthentication();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "WARCHTI V1");
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                //var xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + @"";
            });
        }
    }
}
