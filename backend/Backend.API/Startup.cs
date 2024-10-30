using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Core.Models.Jwt;
using Backend.Core.Repositories;
using Backend.Core.Services;
using Backend.Core.UnitOfWorks;
using Backend.Data;
using Backend.Data.Repositories;
using Backend.Data.UnitOfWorks;
using Backend.Service.Helpers;
using Backend.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Backend.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IService<>), typeof(Service.Services.Service<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICompanyService, CompanyService>();

            services.Configure<JwtOptions>(Configuration.GetSection("JwtOptions"));

            services.AddSingleton<JwtTokenGenerator>();
            services.AddSingleton<PasswordManager>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "auth-deneme", Version = "v1" });
            });

            var jwtKey = Encoding.ASCII.GetBytes(Configuration["JwtOptions:Secret"]);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(
                    JwtBearerDefaults.AuthenticationScheme,
                    options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidIssuer = Configuration["JwtOptions:Issuer"],
                            ValidAudience = Configuration["JwtOptions:Audience"],
                            ClockSkew = TimeSpan.Zero,
                        };
                    }
                );

            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "ShouldBeAdminOrManager",
                    policy => policy.RequireRole("Admin", "Manager")
                );
            });

            var allowedRolesToUpdate = new[] { "Admin", "User" };
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "ShouldBeAdminOrUser",
                    policy =>
                        policy.RequireAssertion(context =>
                            context.User.HasClaim(c =>
                                c.Type == ClaimTypes.Role
                                && allowedRolesToUpdate.Intersect(c.Value.Split(',')).Any()
                            )
                        )
                );
            });

            services.AddControllers();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "AllowSpecificOrigin",
                    builder =>
                    {
                        builder
                            .WithOrigins("http://localhost:3000")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    }
                );
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpsRedirection();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "auth-deneme v1"));
            app.UseHsts();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowSpecificOrigin");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
