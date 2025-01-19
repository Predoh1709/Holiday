using HolidayAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HolidayAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class HolidayController : ControllerBase
  {
    private readonly IHolidayService _holidayService;
    private readonly string _apiUrl;

    public HolidayController(IHolidayService holidayService, IConfiguration configuration)
    {
      _holidayService = holidayService;
      _apiUrl = configuration["NationalHolidaysUrl"];
    }

    [HttpGet("dadosbr-feriados-nacionais")]
    public async Task<IActionResult> GetNationalHolidays()
    {
      try
      {
        var holidays = await _holidayService.FetchAndSaveNationalHolidaysAsync(_apiUrl);
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
      var holidays = await _holidayService.GetAllAsync();
      return Ok(holidays);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetHolidayById(int id)
    {
      var holiday = await _holidayService.GetByIdAsync(id);
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
      var deleted = await _holidayService.DeleteAsync(id);
      if (!deleted) return NotFound("Feriado não encontrado.");
      return NoContent();
    }
  }
}
