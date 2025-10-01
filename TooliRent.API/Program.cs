using FluentValidation;
using TooliRent.Application.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TooliRent.Domain.Models;
using TooliRent.Infrastructure.Data;
using TooliRent.Application.Services;
using TooliRent.Application.Services.Interfaces;
using TooliRent.Domain.RepoInterfaces;
using TooliRent.Infrastructure.Repositories;
using TooliRent.Application.Mapping;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;
using System.Security.Claims;

namespace TooliRent_project
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<TooliRentDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repository pattern
            builder.Services.AddScoped<IToolRepository, ToolRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();


            // Service pattern
            builder.Services.AddScoped<IToolService, ToolService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            //builder.Services.AddScoped<IService, Service>();

            // FluentValidation
            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateToolDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateToolDtoValidator>();
            //builder.Services.AddValidatorsFromAssemblyContaining<CreateToolDtoValidator>();
            //builder.Services.AddFluentValidationAutoValidation();
            //builder.Services.AddFluentValidationClientsideAdapters();


            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateCategoryDtoValidator>();

            builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            builder.Services.AddValidatorsFromAssemblyContaining<BookingReqDtoValidator>();
            builder.Services.AddValidatorsFromAssemblyContaining<UpdateBookingDtoValidator>();




            // AutoMapper
            builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));
            //builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Microsoft Identity
            builder.Services.AddIdentity<User, IdentityRole>(o =>
            {
                o.User.RequireUniqueEmail = true;
                o.Password.RequiredLength = 8;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<TooliRentDbContext>()
                .AddDefaultTokenProviders();

            // JWT
            var jwt = builder.Configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

            //builder.Services
            //    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = jwt["Issuer"],
                        ValidAudience = jwt["Audience"],
                        IssuerSigningKey = key,
                        //ClockSkew = TimeSpan.FromMinutes(1)
                        ClockSkew = TimeSpan.Zero,

                        RoleClaimType = ClaimTypes.Role,
                        NameClaimType = ClaimTypes.NameIdentifier
                    };
                });
            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            // extraa 
            builder.Services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TooliRent", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ange din token"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // seed 
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                await UserSeedData.SeedRolesAndAdminAsync(services);

                await ToolAndRentSeed.SeedCategoriesAndToolsAsync(services);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}