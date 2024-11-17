using TaxCalculator.Models.Api;
using TaxCalculator.Service.Api.Repositories;

namespace TaxCalculator.Service.Api
{
    public class TaxService
    {
        private readonly TaxBandRepository _taxBandRepository;

        public TaxService(TaxBandRepository taxBandRepository) 
        {
            _taxBandRepository = taxBandRepository;
        }

        /// <summary>
        /// Get a list of tax bands given a salary
        /// </summary>
        /// <param name="salary"></param>
        /// <returns></returns>
        public List<BandAndTax> GetTaxAmountForBands(decimal salary, DateTime date)
        {
            var taxBands = _taxBandRepository.GetTaxBandsForDate(date);
            if (taxBands == null || taxBands.Any() == false)
            {
                throw new Exception("No tax bands found for given date");
            }

            var response = new List<BandAndTax>();

            // Loop through all the tax bands, by lower limit
            foreach (var taxBand in taxBands.OrderBy(x => x.LowerLimit))
            {
                var bandAndTax = new BandAndTax(taxBand.Description);

                decimal taxableAmount = 0;

                // Make sure the salary is in the band
                if (salary > taxBand.LowerLimit)
                {
                    if (taxBand.UpperLimit.HasValue)
                    {
                        // Work out how much salary is within this band which has limits
                        taxableAmount = Math.Min(salary, taxBand.UpperLimit.Value) - taxBand.LowerLimit;
                    }
                    else
                    {
                        // All salary over the lower limit (no upper limit)
                        taxableAmount = salary - taxBand.LowerLimit;
                    }
                }
                else
                {
                    // We're in order of lower limit. 
                    // So if salary doesn't go over the lower limit, we wont go over any other lower limits
                    break;
                }

                bandAndTax.TaxAmount = taxableAmount * (taxBand.TaxRate / 100);
                bandAndTax.TaxableAmount = taxableAmount;
                
                response.Add(bandAndTax);
            }

            return response;
        }

        public TaxBreakdownSummary GetTaxBreakdownSummary(decimal salary, List<BandAndTax> taxes)
        {
            // Calculate total taxes
            var totalTax = taxes.Sum(x => x.TaxAmount);

            var summary = new TaxBreakdownSummary
            {
                AnnualTax = totalTax,
                MonthlyTax = totalTax / 12,
                GrossAnnualSalary = salary,
                GrossMonthlySalary = salary / 12,
                TaxBreakdowns = taxes.Select(x => new TaxAndBandSummary
                {
                    Amount = x.TaxAmount,
                    Description = x.Description
                }).ToList()
            };

            return summary;
        }
    }
}
