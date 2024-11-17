using Microsoft.AspNetCore.Mvc;
using TaxCalculator.Models.Api;
using TaxCalculator.Models.Endpoints.Request;
using TaxCalculator.Service.Api;

namespace TaxCalculator.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaxCalculatorController : ControllerBase
    {
        private readonly TaxService _taxService;

        public TaxCalculatorController(TaxService taxService)
        {
            _taxService = taxService;
        }

        /// <summary>
        /// Get the tax breakdown given a salary
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetTaxBreakdown")]
        public TaxBreakdownSummary GetTaxBreakdown([FromBody] GetTaxBreakdownRequest request)
        {
            // Given the salary, get the tax amounts for each band
            var bands = _taxService.GetTaxAmountForBands(request.Salary, DateTime.Now);

            // Now get the breakdown given the tax band breakdown
            var summary = _taxService.GetTaxBreakdownSummary(request.Salary, bands);

            return summary;
        }
    }
}
