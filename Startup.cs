using FlatsAPI.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using FlatsAPI.Models;
using FlatsAPI.Models.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FlatsAPI.Services;
using FlatsAPI.Middleware;
using FlatsAPI.Settings;
using FlatsAPI.Settings.Permissions;
using Microsoft.AspNetCore.Authorization;
using FlatsAPI.Authorization.Policies;
using FlatsAPI.Authorization;
using FlatsAPI.Authorization.Handlers;
using FlatsAPI.Settings.Roles;
using FlatsAPI.Services.Scheduled;

namespace FlatsAPI
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
            // Authentication config

            var authenticationSettings = new AuthenticationSettings();

            Configuration.GetSection("Authentication").Bind(authenticationSettings);

            services.AddSingleton(authenticationSettings);
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = "Bearer";
                option.DefaultScheme = "Bearer";
                option.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = authenticationSettings.JwtIssuer,
                    ValidAudience = authenticationSettings.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
                };
            });

            // Authorization config

            services.AddTransient<IPermissionContext, PermissionContext>();
            services.AddAuthorization();

            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();

            // Roles
            services.AddSingleton<IRole, AdminRole>();
            services.AddSingleton<IRole, LandlordRole>();
            services.AddSingleton<IRole, TenantRole>();

            services.AddControllers().AddFluentValidation();
            services.AddDbContext<FlatsDbContext>();
            
            // Automapper
            services.AddAutoMapper(this.GetType().Assembly);

            // Middlewares
            services.AddScoped<ErrorHandlingMiddleware>();


            // Password hashers
            services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();

            // Validators
            services.AddScoped<IValidator<CreateAccountDto>, CreateAccountDtoValidator>();

            // Services
            services.AddScoped<IUserContextService, UserContextService>();
            services.AddScoped<IBlockOfFlatsService, BlockOfFlatsService>();
            services.AddScoped<IFlatService, FlatService>();
            services.AddScoped<IAccountService, AccountService>();

            // Hosted services
            services.AddHostedService<RentService>();
            
            // Helpers
            services.AddScoped<FlatsSeeder>();

            services.AddHttpContextAccessor();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, FlatsSeeder seeder)
        {

            seeder.Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
