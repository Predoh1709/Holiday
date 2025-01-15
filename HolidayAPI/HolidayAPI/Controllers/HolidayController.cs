using HolidayAPI.DataContext;
using HolidayAPI.Entities;
using HolidayAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json; 

namespace HolidayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly HttpClient _httpClient;

        public HolidayController(ApplicationDbContext dbContext, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _httpClient = httpClient;
        }

        [HttpGet("dadosbr-feriados-nacionais")]
        public async Task<IActionResult> GetNationalHolidays()
        {
            string apiUrl = "http://dadosbr.github.io/feriados/nacionais.json";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var holidays = JsonConvert.DeserializeObject<List<HolidayDTO>>(json);

                    var holidayEntities = holidays.Select(h => new Holiday
                    {
                        Date = h.Date,
                        Title = h.Title,
                        Description = h.Description,
                        Legislation = h.Legislation,
                        Type = h.Type,
                        VariableDates = h.VariableDates.Select(vd => new HolidayVariableDate
                        {
                            Year = int.Parse(vd.Key),
                            Date = vd.Value
                        }).ToList()
                    }).ToList();

                    _dbContext.Holidays.AddRange(holidayEntities);
                    await _dbContext.SaveChangesAsync();

                    return Ok(holidays);
                }

                return StatusCode((int)response.StatusCode, "Erro ao buscar dados da API externa.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHolidays()
        {
            var holidays = await _dbContext.Holidays
                .Include(h => h.VariableDates)
                .ToListAsync();

            return Ok(holidays);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHolidayById(int id)
        {
            var holiday = await _dbContext.Holidays
                .Include(h => h.VariableDates)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (holiday == null)
            {
                return NotFound("Feriado não encontrado.");
            }

            return Ok(holiday);
        }

        [HttpPut("{id}/description")]
        public async Task<IActionResult> UpdateHolidayDescription(int id, [FromBody] string newDescription)
        {
            var holiday = await _dbContext.Holidays.FindAsync(id);

            if (holiday == null)
            {
                return NotFound("Feriado não encontrado.");
            }

            holiday.Description = newDescription;
            await _dbContext.SaveChangesAsync();

            return Ok(holiday);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHoliday(int id)
        {
            var holiday = await _dbContext.Holidays
                .Include(h => h.VariableDates)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (holiday == null)
            {
                return NotFound("Feriado não encontrado.");
            }

            _dbContext.Holidays.Remove(holiday);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}