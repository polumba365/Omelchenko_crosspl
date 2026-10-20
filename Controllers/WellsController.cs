using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omelchenko_crosspl.Models;
using Omelchenko_crosspl.Services;

namespace Omelchenko_crosspl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WellsController : ControllerBase
    {
        private readonly DataService _service;

        // Visual Studio сама передаст сюда наш сервис
        public WellsController(DataService service) => _service = service;

        // Получить все скважины (доступно всем)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var wells = await _service.GetAllWells();
            return Ok(wells);
        }

        // Добавить скважину (ТОЛЬКО ДЛЯ АДМИНА)
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] Well well)
        {
            var created = await _service.CreateWell(well);
            return CreatedAtAction(nameof(GetAll), created);
        }

        // Удалить скважину (ТОЛЬКО ДЛЯ АДМИНА)
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _service.DeleteWell(id))
                return NoContent();
            return NotFound();
        }
    }
}