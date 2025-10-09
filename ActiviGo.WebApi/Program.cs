using ActiviGo.Application.Interfaces;
using ActiviGo.Application.Mapping;
using ActiviGo.Application.Services;
using ActiviGo.Domain.Interfaces;
using ActiviGo.Domain.Models;
using ActiviGo.Infrastructure.Data;
using ActiviGo.Infrastructure.Repositories;
using ActiviGo.WebApi.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace ActiviGo.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });


            builder.Services.AddControllers();

            // -------------------------------
            // Database
            // -------------------------------
            builder.Services.AddDbContext<ActiviGoDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // -------------------------------
            // Geocoding Service
            // -------------------------------
            builder.Services.AddHttpClient<IGeocodingService, GeocodingService>();

            // -------------------------------
            // Email Service
            // -------------------------------
            builder.Services.AddScoped<IEmailService, SmtpEmailService>();

            // -------------------------------
            // Swagger / OpenAPI
            // -------------------------------
            builder.Services.AddSwaggerGen(o =>
            {
                o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                o.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            // -------------------------------
            // Validator
            // -------------------------------
            //builder.Services.AddValidatorsFromAssemblyContaining<>();

            // -------------------------------
            // Identity
            // -------------------------------
            builder.Services.AddIdentityCore<User>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 6;
            })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ActiviGoDbContext>()
                .AddDefaultTokenProviders();

            // -------------------------------
            // JWT Authentication
            // -------------------------------
            var jwtConfig = builder.Configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig["Issuer"],
                        ValidAudience = jwtConfig["Audience"],
                        IssuerSigningKey = key,
                        ClockSkew = TimeSpan.FromMinutes(1) // För att hantera liten tidsskillnad
                    };
                });

            builder.Services.AddAuthorization();

            // -------------------------------
            // Repositories, UnitOfWork & Services
            // -------------------------------
            builder.Services.AddScoped<IUnitofWork, UnitOfWork>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
            builder.Services.AddScoped<IActivityService, ActivityService>();
            builder.Services.AddScoped<IActivityOccurrenceRepository, ActivityOccurrenceRepository>();
            builder.Services.AddScoped<IActivityOccurrenceService, ActivityOccurrenceService>();

            builder.Services.AddAutoMapper(typeof(ActivityProfile).Assembly);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();

        }
    }
}
