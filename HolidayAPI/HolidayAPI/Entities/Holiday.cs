using System.ComponentModel.DataAnnotations;

namespace HolidayAPI.Entities
{
    public class Holiday
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(5)]
        public string Date { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(200)]
        public string Legislation { get; set; }

        [MaxLength(50)]
        public string Type { get; set; }

        public ICollection<HolidayVariableDate> VariableDates { get; set; } = new List<HolidayVariableDate>();
    }
}