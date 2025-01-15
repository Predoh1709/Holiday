using HolidayAPI.DataContext;
using HolidayAPI.Entities;
using HolidayAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HolidayAPI.Services
{
  public class HolidayService
  {
    private readonly ILogger<HolidayService> _logger;
    private readonly ApplicationDbContext _dbContext;
    private readonly HttpClient _httpClient;

    public HolidayService(ILogger<HolidayService> logger, ApplicationDbContext dbContext, HttpClient httpClient)
    {
      _logger = logger;
      _dbContext = dbContext;
      _httpClient = httpClient;
    }

    public async Task<List<HolidayDTO>> FetchAndSaveNationalHolidaysAsync(string apiUrl)
    {
      var holidays = await GetHolidaysFromApi(apiUrl);
      var holidayEntities = new List<Holiday>();

      foreach (var holiday in holidays)
      {
        var existingHoliday = await _dbContext.Holidays
            .FirstOrDefaultAsync(h => h.Title == holiday.Title && h.Description == holiday.Description);

        if (existingHoliday == null)
        {
          var holidayEntity = CreateHolidayEntity(holiday);
          holidayEntities.Add(holidayEntity);
        }
      }

      if (holidayEntities.Any())
      {
        await SaveHolidaysAsync(holidayEntities);
      }

      return holidays;
    }

    private async Task<List<HolidayDTO>> GetHolidaysFromApi(string apiUrl)
    {
      var response = await _httpClient.GetAsync(apiUrl);
      response.EnsureSuccessStatusCode();

      var json = await response.Content.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<List<HolidayDTO>>(json);
    }

    private Holiday CreateHolidayEntity(HolidayDTO holiday)
    {
      return new Holiday
      {
        Date = holiday.Date,
        Title = holiday.Title,
        Description = holiday.Description,
        Legislation = holiday.Legislation,
        Type = holiday.Type,
        VariableDates = holiday.VariableDates.Select(vd => new HolidayVariableDate
        {
          Year = int.Parse(vd.Key),
          Date = vd.Value
        }).ToList()
      };
    }

    private async Task SaveHolidaysAsync(List<Holiday> holidayEntities)
    {
      _dbContext.Holidays.AddRange(holidayEntities);
      await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Holiday>> GetAllHolidaysAsync()
    {
      return await _dbContext.Holidays.Include(h => h.VariableDates).ToListAsync();
    }

    public async Task<Holiday> GetHolidayByIdAsync(int id)
    {
      return await _dbContext.Holidays.Include(h => h.VariableDates).FirstOrDefaultAsync(h => h.Id == id);
    }

    public async Task<Holiday> UpdateHolidayDescriptionAsync(int id, string newDescription)
    {
      var holiday = await _dbContext.Holidays.FindAsync(id);
      if (holiday == null) return null;

      holiday.Description = newDescription;
      await _dbContext.SaveChangesAsync();
      return holiday;
    }

    public async Task<bool> DeleteHolidayAsync(int id)
    {
      var holiday = await _dbContext.Holidays.Include(h => h.VariableDates).FirstOrDefaultAsync(h => h.Id == id);
      if (holiday == null) return false;

      _dbContext.Holidays.Remove(holiday);
      await _dbContext.SaveChangesAsync();
      return true;
    }
  }
}