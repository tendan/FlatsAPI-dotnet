using FlatsAPI;
using FlatsAPI.Authorization.Handlers;
using FlatsAPI.Authorization.Policies;
using FlatsAPI.Entities;
using FlatsAPI.Middleware;
using FlatsAPI.Models.Validators;
using FlatsAPI.Services;
using FlatsAPI.Settings;
using FlatsAPI.Settings.Roles;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Authentication config

var authenticationSettings = new AuthenticationSettings();

builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

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

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Authorization config
services.AddAuthorization();

services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
services.AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();

// Roles
services.AddRolesSingletons();

services.AddControllers();
services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
services.AddDbContext<FlatsDbContext>();

// Automapper
services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Middlewares
services.AddScoped<ErrorHandlingMiddleware>();


// Password hashers
services.AddScoped<IPasswordHasher<Account>, PasswordHasher<Account>>();

// Validators
services.AddValidators();

// Services
services.AddServices();
services.AddSwaggerGen();

// Hosted services
services.AddHostedServices();

// Helpers
services.AddScoped<FlatsSeeder>();

services.AddHttpContextAccessor();


var app = builder.Build();

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<FlatsSeeder>();

seeder.Seed();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flats Of Blocks v1.0"));
}

app.UseMiddleware<ErrorHandlingMiddleware>();


app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
