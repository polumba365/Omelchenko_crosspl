namespace Omelchenko_crosspl.Models
{
    public class MaintenanceRecord
    {
        public int Id { get; set; }
        public int WellId { get; set; }
        public Well? Well { get; set; }

        public int ContractorId { get; set; }
        public ArcticContractor? Contractor { get; set; }

        public DateTime Date { get; set; }
        public decimal ActualCost { get; set; }
        public string Notes { get; set; } = "";
    }
}