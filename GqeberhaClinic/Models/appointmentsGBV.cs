using System.ComponentModel.DataAnnotations;

namespace GqeberhaClinic.Models
{
    public class appointmentsGBV
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Full Names")]
        public string FullName { get; set; }
        [Required]
        [Display(Name = "Purpose of Appointment")]
        public string Purpose { get; set; }
        [Required]
        public DateTime Date { get; set;}
        

    }
}
