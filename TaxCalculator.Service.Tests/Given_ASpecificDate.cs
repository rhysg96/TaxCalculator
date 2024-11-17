using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TaxCalculator.Service.Api;
using TaxCalculator.Service.Api.Repositories;
using TaxCalculator.Service.Data;

namespace TaxCalculator.Service.Tests
{
    public class Given_ASpecificDate
    {
        private TaxService _taxService;
        private TaxBandRepository _taxBandRepository;
        // 0-5000 = 0%
        // 5001-20000 = 20%
        // 20001+ = 40%

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TaxContext>()
                .UseInMemoryDatabase("TaxDatabaseB")
                .Options;

            // Initial setup for the database
            var database = new TaxContext(options);
            var date = new DateTime(2020, 1, 1);

            database.TaxBands.Add(new Models.Entities.TaxBand()
            {
                EffectiveFrom = date,
                LowerLimit = 0,
                UpperLimit = 5000,
                TaxRate = 0
            });

            database.TaxBands.Add(new Models.Entities.TaxBand()
            {
                EffectiveFrom = date,
                LowerLimit = 5001,
                UpperLimit = 20000,
                TaxRate = 20
            });

            database.TaxBands.Add(new Models.Entities.TaxBand()
            {
                EffectiveFrom = date,
                EffectiveTo = date.AddDays(10),
                LowerLimit = 20001,
                UpperLimit = null,
                TaxRate = 40
            });

            database.SaveChanges();


            // Setup the services
            _taxBandRepository = new TaxBandRepository(database);
            _taxService = new TaxService(_taxBandRepository);
        }

        [Test]
        [TestCase("01/01/2022", 2)]
        [TestCase("01/01/2020", 3)]
        public void ThenWeGetTaxBandsForThatDate_Returned(DateTime dateTime, int expectedBands)
        {
            var bands = _taxBandRepository.GetTaxBandsForDate(dateTime).ToList();

            Assert.That(bands.Count, Is.EqualTo(expectedBands));
        }
    }
}