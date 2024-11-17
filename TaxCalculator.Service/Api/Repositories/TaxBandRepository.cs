using TaxCalculator.Service.Data;

namespace TaxCalculator.Service.Api.Repositories
{
    public class TaxBandRepository
    {
        public TaxContext _taxContext;
        public TaxBandRepository(TaxContext taxContext)
        {
            _taxContext = taxContext;
        }

        /// <summary>
        /// Get all the effective tax bands given a specific date
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public IEnumerable<Models.Api.TaxBand> GetTaxBandsForDate(DateTime fromDate)
        {
            var bands = _taxContext.TaxBands
                .Where(x => x.EffectiveFrom <= fromDate && (x.EffectiveTo == null || fromDate <= x.EffectiveTo));

            var apiBands = bands.Select(x => new Models.Api.TaxBand()
            {
                EffectiveTo = x.EffectiveTo,
                EffectiveFrom = x.EffectiveFrom,
                Description = x.Description,
                LowerLimit = x.LowerLimit,
                UpperLimit = x.UpperLimit,
                TaxRate = x.TaxRate
            });

            return apiBands;
        }
    }
}
