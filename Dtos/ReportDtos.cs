namespace Omelchenko_crosspl.Dtos
{
    // DTO для отчета о срочном ремонте
    public class UrgentRepairDto
    {
        public string WellName { get; set; } = "";
        public string FieldName { get; set; } = "";
        public double WearPercentage { get; set; }
        public string RecommendedContractor { get; set; } = "";
    }

    // DTO для отчета по затратам на подрядчиков
    public class ContractorCostDto
    {
        public int ContractorId { get; set; }
        public string ContractorName { get; set; } = "";
        public decimal TotalSpent { get; set; }
        public int RecordsCount { get; set; }
    }

    // DTO для детального отчета по записям
    public class RecordDetailsDto
    {
        public int RecordId { get; set; }
        public string WellName { get; set; } = "";
        public string ContractorName { get; set; } = "";
        public DateTime Date { get; set; }
        public decimal ActualCost { get; set; }
        public decimal EstimatedCost { get; set; } // Расчетная стоимость из бизнес-логики
        public string Notes { get; set; } = "";
    }
}