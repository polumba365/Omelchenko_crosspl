using Microsoft.EntityFrameworkCore;
using Omelchenko_crosspl.Models;

namespace Omelchenko_crosspl.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Таблицы базы данных
        public DbSet<Well> Wells { get; set; }
        public DbSet<ArcticContractor> Contractors { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Тестовые данные (сидирование) — чтобы API не был пустым при запуске

            // Скважины
            modelBuilder.Entity<Well>().HasData(
                new Well
                {
                    Id = 1,
                    Name = "Скважина-101",
                    FieldName = "Новопортовское",
                    Depth = 2500,
                    InstallDate = new DateTime(2020, 5, 10),
                    CurrentTemperature = -35
                },
                new Well
                {
                    Id = 2,
                    Name = "Скважина-102",
                    FieldName = "Юрубчейское",
                    Depth = 3200,
                    InstallDate = new DateTime(2018, 3, 15),
                    CurrentTemperature = -45,
                    LastMaintenanceDate = new DateTime(2021, 1, 1) // Старая дата — для теста срочного ТО
                }
            );

            // Подрядчики
            modelBuilder.Entity<ArcticContractor>().HasData(
                new ArcticContractor
                {
                    Id = 1,
                    CompanyName = "АрктикаСервис",
                    Rating = 5,
                    BasePrice = 50000,
                    HasArcticEquipment = true
                },
                new ArcticContractor
                {
                    Id = 2,
                    CompanyName = "ЭкономРемонт",
                    Rating = 2,
                    BasePrice = 20000,
                    HasArcticEquipment = false
                }
            );

            // Записи о ТО
            modelBuilder.Entity<MaintenanceRecord>().HasData(
                new MaintenanceRecord
                {
                    Id = 1,
                    WellId = 1,
                    ContractorId = 1,
                    Date = new DateTime(2023, 6, 1),
                    ActualCost = 48000,
                    Notes = "Плановая замена клапанов"
                }
            );
        }
    }
}