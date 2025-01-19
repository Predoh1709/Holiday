using HolidayAPI.Entities;
using HolidayAPI.Models;

namespace HolidayAPI.Services
{
  public interface IHolidayService : IService<Holiday, int>
  {
    Task<List<HolidayDTO>> FetchAndSaveNationalHolidaysAsync(string apiUrl);
    Task<Holiday> UpdateHolidayDescriptionAsync(int id, string newDescription);
  }
}
