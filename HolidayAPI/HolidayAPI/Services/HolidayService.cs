using HolidayAPI.DataContext;
using HolidayAPI.Entities;
using HolidayAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HolidayAPI.Services
{
  public class HolidayService
  {
    private readonly ApplicationDbContext _dbContext;
    private readonly HttpClient _httpClient;

    public HolidayService(ApplicationDbContext dbContext, HttpClient httpClient)
    {
      _dbContext = dbContext;
      _httpClient = httpClient;
    }

    public async Task<List<HolidayDTO>> FetchAndSaveNationalHolidaysAsync(string apiUrl)
    {
      var response = await _httpClient.GetAsync(apiUrl);
      response.EnsureSuccessStatusCode();

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

      return holidays;
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
