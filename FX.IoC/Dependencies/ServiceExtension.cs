﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FX.Application.Contracts;
using FX.Application.DataContext;
using FX.Application.EmailService;
using FX.Application.Filters;
using FX.Application.SmsService;
using FX.BackgroundService.ServiceJobs;
using FX.Data.Context;
using FX.DataAccess.UnitOfWork.Implementation;
using FX.DataAccess.UnitOfWork.Interface;
using FX.Infrastructure.Contracts;
using FX.Infrastructure.EmailService;
using FX.Infrastructure.JwtToken;
using FX.Infrastructure.SmsService;

namespace FX.IoC.Dependencies
{
    public static class ServiceExtension
    {
        private readonly static string AnyLocalHostCors = "All_LocalHost";
        private readonly static string SpecificOrigin = "Specific_OriginCORs";
        private readonly static string AnyOrigin = "Any_origin";
        private readonly static string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public static void ConfigureRepositoryAndServices(this IServiceCollection services, IConfiguration Configuration)
        {

            #region REGISTER SERVICES
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //inject scoped services here
            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IBusinessRule, BusinessRule>();
            services.AddScoped<ISqlDBObjects, SqlDBObjects>();

            services.AddTransient<IMailService, MailService>();
            services.AddTransient<ISmsService, SmsService>();



            //inject background services here
            services.AddTransient<IHostedService, SendTestMail>();

            #endregion
        }

        public static void ConfigureModelState(this IServiceCollection services)
        {
            #region ModelState

            services.Configure<ApiBehaviorOptions>(options =>
            {

                options.SuppressModelStateInvalidFilter = true;
            });

            #endregion

        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            #region IDENTITY
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //signin settings
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;

                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            #endregion
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            #region Swagger
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "FX.API",
                    Version = "v1",
                    Description = "FX API",
                    TermsOfService = new Uri("https://ebisike.github.io/resume"),
                    Contact = new OpenApiContact
                    {
                        Name = "FX Services",
                        Email = "support@Selenia.com",
                        Url = new Uri("https://ebisike.github.io/resume"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "FX API LICX",
                        Url = new Uri("https://ebisike.github.io/resume"),
                    }
                });

                opt.OperationFilter<SwaggerHeaderFilter>(); // setting header input for api request

                // XML DOCUMENTATION
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath)) opt.IncludeXmlComments(xmlPath);

                //Enable Jwt Authorization in Swagger
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and your valid token in the text input below. \r\n\r\nExample: \"Bearer eyJhnbGciOrNwi78gGhiLLiUjo9A8dXCVBk9\""
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
            #endregion
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration Configuration)
        {
            #region JWT
            var TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = Configuration["JwtSettings:Audience"],
                ValidIssuer = Configuration["JwtSettings:Site"],
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:Secret"])),
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(TokenValidationParameters);

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = TokenValidationParameters;
            });
            #endregion
        }

        /// <summary>
        /// configure CORS for specific origin
        /// </summary>
        /// <param name="services"></param>
        /// <param name="origins"></param>
        /// <returns>policy name</returns>
        public static string ConfigureCorsSpecificOrigin(this IServiceCollection services, string[] origins)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: SpecificOrigin, builder => builder.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod());
            });

            return SpecificOrigin;
        }

        public static void ConfigureCORS(this IServiceCollection services, IConfiguration Configuration)
        {
            #region Cors
            /*

            services.AddCors(options =>
            {
                options.AddPolicy(AnyLocalHostCors, builder => builder.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback).AllowAnyHeader().AllowAnyMethod());
            });
            services.AddCors(options =>
            {
                options.AddPolicy(name: SpecificOrigin, builder => builder.WithOrigins(Configuration["CorsOrigin:WebApp"],
                                                                                           Configuration["CorsOrigin:MobileApp"]).AllowAnyHeader().AllowAnyMethod());
            }); */

            services.AddCors(options =>
            {
                options.AddPolicy(name: AnyOrigin, builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            #endregion
        }

        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            #region API VERSIONING
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true; //this will cause our api to default to verion 1.0

                //Default verion 1.0
                options.DefaultApiVersion = ApiVersion.Default; //new ApiVersion(1, 0); //ApiVersion.Default;

                options.ApiVersionReader = ApiVersionReader.Combine(new MediaTypeApiVersionReader("version"),
                                                                    new HeaderApiVersionReader("x-api-version"));

                options.ReportApiVersions = true; // Will provide the different api version which is available for the client
            });
            #endregion
        }
        public static void ConfigureDatabaseConnection(this IServiceCollection services, IConfiguration Configuration)
        {
            #region DB Connection
            try
            {
                if (env == "Production")
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseMySql(connectionString: Environment.GetEnvironmentVariable("WebApiDatabase_Production"), serverVersion: ServerVersion.AutoDetect(Environment.GetEnvironmentVariable("WebApiDatabase_Production")), mySqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                    });
                }

                if (env == "Staging")
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseMySql(connectionString: Environment.GetEnvironmentVariable("WebApiDatabase_Staging"), serverVersion: ServerVersion.AutoDetect(Environment.GetEnvironmentVariable("WebApiDatabase_Staging")), mySqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                    });
                }

                if (env == "Development")
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseMySql(connectionString: Configuration.GetConnectionString("WebApiDatabase_Development"), serverVersion: ServerVersion.AutoDetect(Configuration.GetConnectionString("WebApiDatabase_Development")), mySqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                    });
                }
            }
            catch (Exception)
            {

            }

            //FX dbcontext
            try
            {
                if (env == "Production")
                {
                    services.AddDbContext<FXDbContext>(options =>
                    {
                        options.UseMySql(connectionString: Environment.GetEnvironmentVariable("WebApiDatabase_Production"), serverVersion: ServerVersion.AutoDetect(Environment.GetEnvironmentVariable("WebApiDatabase_Production")), mySqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                    });
                }

                if (env == "Staging")
                {
                    services.AddDbContext<FXDbContext>(options =>
                    {
                        options.UseMySql(connectionString: Environment.GetEnvironmentVariable("WebApiDatabase_Staging"), serverVersion: ServerVersion.AutoDetect(Environment.GetEnvironmentVariable("WebApiDatabase_Staging")), mySqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                    });
                }

                if (env == "Development")
                {
                    services.AddDbContext<FXDbContext>(options =>
                    {
                        options.UseMySql(connectionString: Configuration.GetConnectionString("WebApiDatabase_Development"), serverVersion: ServerVersion.AutoDetect(Configuration.GetConnectionString("WebApiDatabase_Development")), mySqlOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                            sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        });
                    });
                }
            }
            catch (Exception)
            {

            }
            #endregion
        }
    }
}
