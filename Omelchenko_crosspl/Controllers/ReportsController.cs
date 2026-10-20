using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omelchenko_crosspl.Services;

namespace Omelchenko_crosspl.Controllers
{
    // Весь контроллер доступен только авторизованным пользователям
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly DataService _service;
        public ReportsController(DataService service) => _service = service;

        // Отчет 1: Скважины, требующие срочного ремонта
        [HttpGet("urgent")]
        public IActionResult GetUrgentRepairs()
        {
            var result = _service.GetUrgentRepairs();
            return Ok(result);
        }

        // Отчет 2: Затраты по каждому подрядчику (с группировкой)
        [HttpGet("costs")]
        public IActionResult GetContractorCosts()
        {
            var result = _service.GetContractorCosts();
            return Ok(result);
        }

        // Отчет 3: Детализация всех записей о ТО с расчетной стоимостью
        [HttpGet("details")]
        public IActionResult GetAllRecordsDetails()
        {
            var result = _service.GetAllRecordsDetails();
            return Ok(result);
        }
    }
}