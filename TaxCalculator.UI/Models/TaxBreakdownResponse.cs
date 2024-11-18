namespace TaxCalculator.UI.Models
{
    public class TaxBreakdownResponse
    {
        public decimal AnnualTax { get; set; }
        public decimal MonthlyTax { get; set; }
        public decimal GrossAnnualSalary { get; set; }
        public decimal GrossMonthlySalary { get; set; }
        public decimal NetAnnualSalary { get; set; }
        public decimal NetMonthlySalary { get; set; }

        public List<TaxBandBreakdown> TaxBreakdowns { get; set; }
    }

    public class TaxBandBreakdown
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
