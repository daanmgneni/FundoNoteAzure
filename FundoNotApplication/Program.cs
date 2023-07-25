using FundoNotApplication.Context;
using FundoNotApplication.Interface;
using FundoNotApplication.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nest;
using Serilog;
using System.Security.Permissions;
using System.Text;

namespace FundoNotApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please Enter your token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            builder.Services.AddDbContext<FundooContext>(options =>
            {
                var cosmosDbSetting = builder.Configuration.GetSection("CosmosDb");
                options.UseCosmos(cosmosDbSetting["EndpointUri"], cosmosDbSetting["PrimaryKey"], cosmosDbSetting["DatabaseName"]);
            });
            builder.Services.AddScoped<IUser, UserService>();
            builder.Services.AddScoped<INotes, NoteService>();  
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT-Key"])),
                };
            });

            var elasticUri = new Uri(builder.Configuration.GetSection("ElasticsearchSettings:Uri").Value);
            var settings = new ConnectionSettings(elasticUri);
            var elasticClient = new ElasticClient(settings);
            builder.Services.AddSingleton<IElasticClient>(elasticClient);


            builder.Services.AddLogging(options =>
            {
                options.ClearProviders();
                options.AddSerilog();
            });

            var app = builder.Build();


            var loggingConfiguration = new LoggerConfiguration()
                .MinimumLevel.Error()
                .Enrich.FromLogContext()
                .WriteTo.Console();
            Serilog.Log.Logger = loggingConfiguration.CreateLogger();

                app.UseSwagger();
                app.UseSwaggerUI();
            

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}