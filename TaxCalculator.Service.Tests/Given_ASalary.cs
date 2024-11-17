using Microsoft.EntityFrameworkCore;
using TaxCalculator.Service.Api.Repositories;
using TaxCalculator.Service.Api;
using TaxCalculator.Service.Data;
using TaxCalculator.Service.Controllers;

namespace TaxCalculator.Service.Tests
{
    internal class Given_ASalary
    {
        private TaxCalculatorController _taxController;
        private TaxService _taxService;
        private TaxBandRepository _taxBandRepository;
        // 0-5000 = 0%
        // 5001-20000 = 20%
        // 20001+ = 40%

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TaxContext>()
                .UseInMemoryDatabase("TaxDatabaseA")
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
                LowerLimit = 5000,
                UpperLimit = 20000,
                TaxRate = 20
            });

            database.TaxBands.Add(new Models.Entities.TaxBand()
            {
                EffectiveFrom = date,
                LowerLimit = 20000,
                UpperLimit = null,
                TaxRate = 40
            });

            database.SaveChanges();


            // Setup the services
            _taxBandRepository = new TaxBandRepository(database);
            _taxService = new TaxService(_taxBandRepository);

            _taxController = new TaxCalculatorController(_taxService);
        }

        [Test]
        [TestCase(1000)]
        [TestCase(5000)]
        [TestCase(20000)]
        [TestCase(50000)]
        public void ThenTheSummaryShouldContainTheGivenSalary(decimal salary)
        {
            var breakdown = _taxController.GetTaxBreakdown(new Models.Endpoints.Request.GetTaxBreakdownRequest
            {
                Salary = salary,
                Date = DateTime.Now
            });

            Assert.That(breakdown.GrossAnnualSalary, Is.EqualTo(salary));
        }


        [Test]
        [TestCase(1000, 0)]
        [TestCase(5000, 0)]
        [TestCase(10000, 1000)]
        [TestCase(40000, 11000)]
        public void ThenTheTaxAmountShouldBe(decimal salary, decimal expectedTax)
        {
            var breakdown = _taxController.GetTaxBreakdown(new Models.Endpoints.Request.GetTaxBreakdownRequest
            {
                Salary = salary,
                Date = DateTime.Now
            });

            Assert.That(breakdown.AnnualTax, Is.EqualTo(expectedTax));
        }

        [Test]
        [TestCase(1200, 100)]
        [TestCase(2400, 200)]
        public void ThenTheMonthlyAmountShouldBe(decimal salary, decimal expectedMonthlyAmount)
        {
            var breakdown = _taxController.GetTaxBreakdown(new Models.Endpoints.Request.GetTaxBreakdownRequest
            {
                Salary = salary,
                Date = DateTime.Now
            });

            Assert.That(breakdown.GrossMonthlySalary, Is.EqualTo(expectedMonthlyAmount));
        }
    }
}
