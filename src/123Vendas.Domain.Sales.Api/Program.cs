using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace _123Vendas.Domain.Sales.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            var elasticUri = builder.Configuration["Elasticsearch:Uri"] ?? "http://localhost:9200";
            var elasticUsername = builder.Configuration["Elasticsearch:Username"];
            var elasticPassword = builder.Configuration["Elasticsearch:Password"];

            var loggerConfig = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentName()
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("Application", "123Vendas.Domain.Sales")
                .WriteTo.Console();

            var elasticOptions = new ElasticsearchSinkOptions(new Uri(elasticUri))
            {
                AutoRegisterTemplate = true,
                IndexFormat = "123vendas-sales-{0:yyyy.MM.dd}",
                NumberOfShards = 2,
                NumberOfReplicas = 1
            };

            // Only add authentication if credentials are provided
            if (!string.IsNullOrEmpty(elasticUsername) && !string.IsNullOrEmpty(elasticPassword))
            {
                elasticOptions.ModifyConnectionSettings = x => x.BasicAuthentication(elasticUsername, elasticPassword);
            }

            Log.Logger = loggerConfig.WriteTo.Elasticsearch(elasticOptions).CreateLogger();

            builder.Host.UseSerilog();

            // Add services to the container.
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(_123Vendas.Domain.Sales.Application.UsesCase.Sale.Create.CreateSaleCommand).Assembly);
            });

            // Register DbContext
            builder.Services.AddDbContext<_123Vendas.Domain.Sales.Database.SalesDbContext>(options =>
            {
                options.UseInMemoryDatabase("SalesDb");
            });

            // Register Repositories - Isso pode vir de uma classe especifica de DI, eu deixei assim para adiantar o teste.
            builder.Services.AddScoped<_123Vendas.Domain.Sales.Domain.Repositories.ISaleRepository,
                                       _123Vendas.Domain.Sales.Database.Repositories.SaleRepository>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            try
            {
                Log.Information("Starting 123Vendas Domain Sales API");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
