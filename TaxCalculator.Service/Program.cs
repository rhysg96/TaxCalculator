using Microsoft.EntityFrameworkCore;
using TaxCalculator.Models.Api;
using TaxCalculator.Service.Api;
using TaxCalculator.Service.Api.Repositories;
using TaxCalculator.Service.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaxCalculator.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure DI services
            // Database setup
            builder.Services.AddDbContext<TaxContext>(ops => ops.UseInMemoryDatabase("TaxBands"));
            builder.Services.AddScoped<TaxService>();
            builder.Services.AddScoped<TaxBandRepository>();
            
            var app = builder.Build();

            SetupDatabase(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        private static void SetupDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                // This is just for testing/illustration purposes

                var database = scope.ServiceProvider.GetRequiredService<TaxContext>();
                var date = new DateTime(2000, 1, 1);

                database.TaxBands.Add(new Models.Entities.TaxBand()
                {
                    Description = "Band A",
                    EffectiveFrom = date,
                    LowerLimit = 0,
                    UpperLimit = 5000,
                    TaxRate = 0
                });

                database.TaxBands.Add(new Models.Entities.TaxBand()
                {
                    Description = "Band B",
                    EffectiveFrom = date,
                    LowerLimit = 5000,
                    UpperLimit = 20000,
                    TaxRate = 20
                });

                database.TaxBands.Add(new Models.Entities.TaxBand()
                {
                    Description = "Band C",
                    EffectiveFrom = date,
                    LowerLimit = 20000,
                    UpperLimit = null,
                    TaxRate = 40
                });

                database.SaveChanges();
            }
        }
    }
}
