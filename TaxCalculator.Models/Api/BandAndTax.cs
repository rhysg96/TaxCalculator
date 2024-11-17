namespace TaxCalculator.Models.Api
{
    public class BandAndTax
    {
        public BandAndTax(string description)
        {
            Description = description;
        }

        public string Description { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal TaxAmount { get; set; }
    }
}
