using Microsoft.EntityFrameworkCore;
using Omelchenko_crosspl.Data;
using Omelchenko_crosspl.Dtos;
using Omelchenko_crosspl.Models;

namespace Omelchenko_crosspl.Services
{
    public class DataService
    {
        private readonly AppDbContext _context;

        // Конструктор: сюда Visual Studio сама подставит базу данных
        public DataService(AppDbContext context) => _context = context;

        // === CRUD для Скважин (Well) ===
        public async Task<List<Well>> GetAllWells() => await _context.Wells.ToListAsync();

        public async Task<Well> CreateWell(Well well)
        {
            _context.Wells.Add(well);
            await _context.SaveChangesAsync();
            return well;
        }

        public async Task<bool> DeleteWell(int id)
        {
            var w = await _context.Wells.FindAsync(id);
            if (w == null) return false;
            _context.Wells.Remove(w);
            await _context.SaveChangesAsync();
            return true;
        }

        // === CRUD для Подрядчиков (Contractor) ===
        public async Task<List<ArcticContractor>> GetAllContractors() => await _context.Contractors.ToListAsync();

        public async Task<ArcticContractor> CreateContractor(ArcticContractor c)
        {
            _context.Contractors.Add(c);
            await _context.SaveChangesAsync();
            return c;
        }

        public async Task<bool> DeleteContractor(int id)
        {
            var c = await _context.Contractors.FindAsync(id);
            if (c == null) return false;
            _context.Contractors.Remove(c);
            await _context.SaveChangesAsync();
            return true;
        }

        // === CRUD для Записей о ТО (MaintenanceRecord) ===
        public async Task<List<MaintenanceRecord>> GetAllRecords() =>
            await _context.MaintenanceRecords.Include(r => r.Well).Include(r => r.Contractor).ToListAsync();

        public async Task<MaintenanceRecord> CreateRecord(MaintenanceRecord r)
        {
            _context.MaintenanceRecords.Add(r);
            await _context.SaveChangesAsync();
            return r;
        }

        public async Task<bool> DeleteRecord(int id)
        {
            var r = await _context.MaintenanceRecords.FindAsync(id);
            if (r == null) return false;
            _context.MaintenanceRecords.Remove(r);
            await _context.SaveChangesAsync();
            return true;
        }

        // === СВОДНЫЕ ОТЧЕТЫ (LINQ: Where, Select, GroupBy, Join) ===

        // 1. Срочный ремонт (объединение Well и Contractor через LINQ)
        public List<UrgentRepairDto> GetUrgentRepairs()
        {
            var contractors = _context.Contractors.ToList();
            return _context.Wells.ToList()
                .Where(w => w.RequiresUrgentMaintenance()) // LINQ Where + бизнес-логика
                .Select(w => new UrgentRepairDto // LINQ Select (строгий DTO)
                {
                    WellName = w.Name,
                    FieldName = w.FieldName,
                    WearPercentage = w.CalculateWearPercentage(),
                    RecommendedContractor = contractors
                        .Where(c => c.Rating >= 4 && c.HasArcticEquipment)
                        .OrderBy(c => c.BasePrice)
                        .Select(c => c.CompanyName)
                        .FirstOrDefault() ?? "Нет"
                }).ToList();
        }

        // 2. Затраты по подрядчикам (LINQ GroupBy)
        public List<ContractorCostDto> GetContractorCosts()
        {
            return _context.MaintenanceRecords.Include(r => r.Contractor)
                .GroupBy(r => r.ContractorId) // LINQ GroupBy
                .Select(g => new ContractorCostDto // LINQ Select
                {
                    ContractorId = g.Key,
                    ContractorName = g.First().Contractor!.CompanyName,
                    TotalSpent = g.Sum(r => r.ActualCost),
                    RecordsCount = g.Count()
                }).ToList();
        }

        // 3. Детализация записей (LINQ Join всех 3 таблиц через Include)
        public List<RecordDetailsDto> GetAllRecordsDetails()
        {
            return _context.MaintenanceRecords
                .Include(r => r.Well)
                .Include(r => r.Contractor)
                .ToList()
                .Select(r => new RecordDetailsDto // LINQ Select
                {
                    RecordId = r.Id,
                    WellName = r.Well!.Name,
                    ContractorName = r.Contractor!.CompanyName,
                    Date = r.Date,
                    ActualCost = r.ActualCost,
                    EstimatedCost = r.Contractor.CalcCost(r.Well), // Вызов бизнес-логики
                    Notes = r.Notes
                }).ToList();
        }
    }
}