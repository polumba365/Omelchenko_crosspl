namespace Omelchenko_crosspl.Models
{
    public class Well
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string FieldName { get; set; } = ""; // Месторождение
        public double Depth { get; set; } // Глубина, м
        public DateTime InstallDate { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public double CurrentTemperature { get; set; } // °C

        // БИЗНЕС-ЛОГИКА: Расчет износа (в Арктике холод ускоряет износ)
        public double CalculateWearPercentage()
        {
            int years = DateTime.Now.Year - InstallDate.Year;
            double wear = years * 3.0; // 3% в год
            if (CurrentTemperature < -40) wear += 5.0; // Доп. износ от холода
            return Math.Min(wear, 100);
        }

        // БИЗНЕС-ЛОГИКА: Нужен ли срочный ремонт?
        public bool RequiresUrgentMaintenance()
        {
            if (CalculateWearPercentage() > 75) return true;
            if (LastMaintenanceDate == null) return true;
            return (DateTime.Now - LastMaintenanceDate.Value).TotalDays > 730; // 2 года для Арктики
        }
    }
}