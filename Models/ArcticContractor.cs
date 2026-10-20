namespace Omelchenko_crosspl.Models
{
    public class ArcticContractor
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = "";
        public int Rating { get; set; } // 1-5
        public decimal BasePrice { get; set; }
        public bool HasArcticEquipment { get; set; }

        // БИЗНЕС-ЛОГИКА: Расчет стоимости ТО
        public decimal CalcCost(Well well)
        {
            decimal depthFactor = well.Depth > 3000 ? 1.5m : 1.0m;
            decimal ratingDiscount = Rating >= 4 ? 0.95m : 1.0m;
            decimal arcticSurcharge = HasArcticEquipment ? 1.0m : 1.3m; // Наценка 30% без спецтехники
            return BasePrice * depthFactor * ratingDiscount * arcticSurcharge;
        }

        // БИЗНЕС-ЛОГИКА: Дата следующего ТО
        public DateTime CalcNextMainDate(DateTime lastDate) => lastDate.AddYears(2);
    }
}