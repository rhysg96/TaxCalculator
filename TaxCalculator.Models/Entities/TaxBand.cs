namespace TaxCalculator.Models.Entities
{
    public class TaxBand
    {
        public int TaxBandId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string? Description { get; set; }
        public decimal LowerLimit { get; set; }
        public decimal? UpperLimit { get; set; }
        public decimal TaxRate { get; set; }
    }
}
