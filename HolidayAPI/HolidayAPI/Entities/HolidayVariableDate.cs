using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HolidayAPI.Entities
{
    public class HolidayVariableDate
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [MaxLength(5)]
        public string Date { get; set; }
        
        [ForeignKey("Holiday")]
        public int HolidayId { get; set; }
        public Holiday Holiday { get; set; }
    }
}