using App.Abstrations;
using App.Services;
using Domain.Context;
using Domain.Entities;
using Domain.Models;
using Domain.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using IdentityRole = Domain.Entities.IdentityRole;

namespace Web.Extensions
{
    public static class ServiceExtensions
    {
        public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Orders Api",
                    Version = "v1"
                });
                ///Авторизация на swagger Bearer JWT
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter a valid token",
                    Name = "Autorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        Array.Empty<string>()
                    }
                });

            });

            return builder;
        }
        public static WebApplicationBuilder AddData(this WebApplicationBuilder builder) 
        {
            builder.Services.AddDbContext<OrdersDbContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 34))));
          

            return builder;

        }
        public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICartsService,CartService>();
            builder.Services.AddScoped<IOrdersService, OrderService>();
            return builder;
        }
        public static WebApplicationBuilder AddIntegrationServices(this WebApplicationBuilder builder)
        {
            return builder;
        }
        public static WebApplicationBuilder AddBearerAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {///схема аутентификации
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.UseSecurityTokenValidators = true;
                ///параметры для валидации токена
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Autentication:TokenPrivateKey"]!)),
                    ValidIssuer = "test",
                    ValidAudience = "test",
                    // ValidateIssuer = true,
                    // ValidateAudience = true,
                    // ValidateLifetime = true,
                    // ValidateIssuerSigningKey = true
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = false
                };
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole(RoleConsts.Admin));
                options.AddPolicy("Merchant", policy => policy.RequireRole(RoleConsts.Merchant));
                options.AddPolicy("User", policy => policy.RequireRole(RoleConsts.User));
            });
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddDefaultIdentity<UserEntity>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<OrdersDbContext>()
            .AddUserManager<UserManager<UserEntity>>()
            .AddUserStore<UserStore<UserEntity, IdentityRole, OrdersDbContext, long>>();

            return builder;
        }
        public static WebApplicationBuilder AddOptions(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<AuthOptions>(builder.Configuration.GetSection("Autentication"));
            return builder;
        }


    }
}
