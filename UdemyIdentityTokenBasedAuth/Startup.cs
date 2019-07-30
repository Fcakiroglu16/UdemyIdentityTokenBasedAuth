using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UdemyIdentityTokenBasedAuth.Domain.Services;
using UdemyIdentityTokenBasedAuth.Models;
using UdemyIdentityTokenBasedAuth.Security.Token;
using UdemyIdentityTokenBasedAuth.Services;

namespace UdemyIdentityTokenBasedAuth
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
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();


            services.AddCors(opts =>
            {

                opts.AddDefaultPolicy(builder =>
                {

                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });




            services.AddDbContext<AppIdentityDbContext>(opts =>
            {
                opts.UseSqlServer(Configuration["ConnectionStrings:DefaultConnectionString"]);
              
            });

            services.AddIdentity<AppUser, AppRole>(opts =>
            {
                //https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.useroptions.allowedusernamecharacters?view=aspnetcore-2.2

                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = "abcçdefgğhıijklmnoçpqrsştuüvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._";

                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();

            services.Configure<CustomTokenOptions>(Configuration.GetSection("TokenOptions"));
            var tokenOptions = Configuration.GetSection("TokenOptions").Get<CustomTokenOptions>();

            services.AddAuthentication(opts=> {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtbeareroptions =>
            {
                jwtbeareroptions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                   IssuerSigningKey = SignHandler.GetSecurityKey(tokenOptions.SecurityKey),
                    ClockSkew = TimeSpan.Zero
                };
            });




;









            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseStaticFiles();


            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
