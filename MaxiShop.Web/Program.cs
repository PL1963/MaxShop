using MaxiShop.Infrastructure;
using MaxiShop.Application;
using MaxiShop.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using MaxiShop.Infrastructure.Common;
using MaxiShop.Web.MiddleWares;
using Microsoft.AspNetCore.Identity;
using MaxiShop.Application.Comman;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;



namespace MaxiShop.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddApplicationServices();

            builder.Services.AddInfrastructureServices();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Custompolicy",
                                  policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            });

            #region
            builder.Services.
                AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            #endregion

            builder.Services.AddResponseCaching();

            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            builder.Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;

            });


            builder.Services.AddControllers(options =>
            {
                options.CacheProfiles.Add("Default", new CacheProfile
                {
                    Duration = 30
                });
            });

            builder.Host.UseSerilog((context , config) =>
            {
                config.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day);

                if(context.HostingEnvironment.IsProduction() == false)
                {
                    config.WriteTo.Console();
                }
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["JwtSetting:Issuer"],
                    ValidAudience = builder.Configuration["JwtSetting:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSetting:Key"]))
                };
            });

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                { 
                    Title = "Maxi Shop Version 1",
                    Description = "Developed By stanli",
                    Version = "v1.0",
                
                });

                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Maxi Shop Version 2",
                    Description = "Developed By stanli",
                    Version = "v2.0",

                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    Description = @"Jwt Authorization header using the Bearer Scheme.
                                    Enter 'Bearer' [space] and then your token in the input below.
                                    Example: 'Bearer 1234abcdf'",
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
                            },
                            Scheme = "Oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });


            });

            #region  configuration for seeding data to database
            static async void UpdateDatabaseAsync(IHost host)
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<ApplicationDbContext>();

                        if (context.Database.IsSqlServer())
                        {
                            context.Database.Migrate();
                        }
                        await SeedData.SeedDataAsync(context);
                    }
                    catch (Exception ex)
                    {
                        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                        logger.LogError(ex, "An error occurred while migrating or seeding the database");

                    }
                }
            }

            #endregion

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleWare>();

            UpdateDatabaseAsync(app);

            var serviceProvider = app.Services;

            await SeedData.SeedRoles(serviceProvider);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "MaxiShop_V1");
                    options.SwaggerEndpoint("/swagger/v2/swagger.json", "MaxiShop_V2");
                });
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MaxiShop_V1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "MaxiShop_V2");
                options.RoutePrefix = string.Empty;
            });

            app.UseCors("Custompolicy");


            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
