using System.ComponentModel.DataAnnotations;

namespace GqeberhaClinic.Models
{
    public class Vaccine_Education
    {
        [Key]
        public int EducationID { get; set; }

        [Required]
        public string? VaccineName { get; set; }

        [Required]
        public string? VaccineDescription { get; set; }
        [Required]
        public string? Purpose { get; set; }
        [Required]
        public byte[]? Image { get; set; }
      

    }
}
