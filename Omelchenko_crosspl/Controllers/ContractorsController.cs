using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Omelchenko_crosspl.Models;
using Omelchenko_crosspl.Services;

namespace Omelchenko_crosspl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractorsController : ControllerBase
    {
        private readonly DataService _service;
        public ContractorsController(DataService service) => _service = service;

        // Получить всех подрядчиков
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contractors = await _service.GetAllContractors();
            return Ok(contractors);
        }

        // Добавить подрядчика (ТОЛЬКО ДЛЯ АДМИНА)
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] ArcticContractor c)
        {
            var created = await _service.CreateContractor(c);
            return CreatedAtAction(nameof(GetAll), created);
        }

        // Удалить подрядчика (ТОЛЬКО ДЛЯ АДМИНА)
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _service.DeleteContractor(id))
                return NoContent();
            return NotFound();
        }
    }
}