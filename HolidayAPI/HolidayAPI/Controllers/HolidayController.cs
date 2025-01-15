using HolidayAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HolidayAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class HolidayController : ControllerBase
  {
    private readonly HolidayService _holidayService;

    public HolidayController(HolidayService holidayService)
    {
      _holidayService = holidayService;
    }

    [HttpGet("dadosbr-feriados-nacionais")]
    public async Task<IActionResult> GetNationalHolidays()
    {
      try
      {
        string apiUrl = "http://dadosbr.github.io/feriados/nacionais.json";
        var holidays = await _holidayService.FetchAndSaveNationalHolidaysAsync(apiUrl);
        return Ok(holidays);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
      }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllHolidays()
    {
      var holidays = await _holidayService.GetAllHolidaysAsync();
      return Ok(holidays);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHolidayById(int id)
    {
      var holiday = await _holidayService.GetHolidayByIdAsync(id);
      if (holiday == null) return NotFound("Feriado não encontrado.");
      return Ok(holiday);
    }

    [HttpPut("{id}/description")]
    public async Task<IActionResult> UpdateHolidayDescription(int id, [FromBody] string newDescription)
    {
      if (string.IsNullOrWhiteSpace(newDescription))
        return BadRequest("A descrição não pode ser nula ou vazia.");

      var holiday = await _holidayService.UpdateHolidayDescriptionAsync(id, newDescription);
      if (holiday == null) return NotFound("Feriado não encontrado.");
      return Ok(holiday);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHoliday(int id)
    {
      var deleted = await _holidayService.DeleteHolidayAsync(id);
      if (!deleted) return NotFound("Feriado não encontrado.");
      return NoContent();
    }
  }
}