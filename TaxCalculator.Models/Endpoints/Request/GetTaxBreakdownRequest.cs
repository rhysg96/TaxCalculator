namespace TaxCalculator.Models.Endpoints.Request
{
    public class GetTaxBreakdownRequest
    {
        public DateTime Date { get; set; }
        public decimal Salary { get; set; }
    }
}
