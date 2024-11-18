namespace TaxCalculator.Models.Api
{
    public class TaxAndBandSummary
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }

    public class TaxBreakdownSummary
    {
        /// <summary>
        /// Annual salary before tax
        /// </summary>
        public decimal GrossAnnualSalary { get; set; }

        /// <summary>
        /// Monthly salary before tax
        /// </summary>
        public decimal GrossMonthlySalary { get; set; }
        
        /// <summary>
        /// Tax paid over the whole year
        /// </summary>
        public decimal AnnualTax { get; set; }
       
        /// <summary>
        /// Tax paid per month
        /// </summary>
        public decimal MonthlyTax { get; set; }

        /// <summary>
        /// Salary after taxes
        /// </summary>
        public decimal NetAnnualSalary { get; set; }
        
        /// <summary>
        /// Monthly salary after taxes
        /// </summary>
        public decimal NetMonthlySalary { get; set; }

        /// <summary>
        /// The tax breakdown per band
        /// </summary>
        public List<TaxAndBandSummary> TaxBreakdowns { get; set; }
    }
}
