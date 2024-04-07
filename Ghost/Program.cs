using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Ghost.Extensions.Options;
using Ghost.Models;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using RestSharp;
using Tonisoft.AspExtensions.Cors;
using Tonisoft.AspExtensions.Email;
using Tonisoft.AspExtensions.Module;

namespace Ghost;

public class Program
{
    public static void Main(string[] args)
    {
        Logger? logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddUnitOfWork<PrimaryDbContext>();
            ConfigureDatabase<PrimaryDbContext>(builder.Services, builder.Configuration);

            // Add services to the container.
            builder.Services.RegisterModules(typeof(Program));

            var autoMapperConfig = new MapperConfiguration(config => { config.AddProfile(new AutoMapperProfile()); });
            builder.Services.AddSingleton(autoMapperConfig.CreateMapper());

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.AddCors(CorsOptions.CorsSection);

            builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection(EmailOptions.EmailSection));
            builder.Services.Configure<ConfigOptions>(builder.Configuration.GetSection(ConfigOptions.ConfigSection));

            builder.Services.AddSingleton<IRestClient>(
                new RestClient(new RestClientOptions("https://zenquotes.io/api/") {
                    MaxTimeout = 5000
                }));

            if (!builder.Environment.IsDevelopment())
            {
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();
            }

            WebApplication app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();
            app.UseCors(CorsOptions.CorsPolicyName);
            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Stopped program because of exception");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    private static void ConfigureDatabase<TContext>(IServiceCollection services, IConfiguration configuration)
        where TContext : DbContext
    {
        string database = configuration.GetConnectionString("Database") ?? throw new Exception("Missing database");
        string connection = configuration.GetConnectionString("DefaultConnection") ??
                            throw new Exception("Missing database connection");

        switch (database)
        {
            case "MySQL":
                services.AddDbContext<TContext>(option => { option.UseMySQL(connection); });
                break;
            case "SQLite":
                services.AddDbContext<TContext>(option => { option.UseSqlite(connection); });
                break;
            default:
                throw new Exception($"Invalid database: {database}");
        }
    }
}