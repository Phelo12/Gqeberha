using System.ComponentModel.DataAnnotations;

namespace GqeberhaClinic.Models
{
    public class ReportCase
    {
        [Key] public int caseID { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]

        public string Address { get; set; }
        [Required]
        [Display(Name ="How would you like for us to contact you?")]
        public string Communication { get; set; }
        [Required]
        [Display(Name = "When would you like for us to contact you?")]

        public string Time { get; set; }
        [Required]

        public string caseDescription { get; set; }

    }
}
