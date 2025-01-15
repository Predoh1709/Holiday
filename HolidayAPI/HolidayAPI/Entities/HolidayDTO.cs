namespace HolidayAPI.Models
{
    public class HolidayDTO
    {
        public string Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Legislation { get; set; }
        public string Type { get; set; }
        public Dictionary<string, string>  VariableDates { get; set; }
    }
}
