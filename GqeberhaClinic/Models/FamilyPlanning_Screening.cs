using GqeberhaClinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GqeberhaClinic.Models
{
    public class FamilyPlanning_Screening
    {
        [Key]
        public int ScreeningID { get; set; }

        [Display(Name ="Date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        public string? PatientID { get; set; }
        [ForeignKey("PatientID")]
        public virtual GqebheraUser? MainUser { get; set; }
        public string? Status { get; set; }



        //Questions
        [Display(Name = "1. Are you Drinking?")]
        [Required]
        public int? Question1 { get; set; }

        [Display(Name = "2. Do you Smoke?")]
        [Required]
        public int? Question2 { get; set; }

        [Display(Name = "3. Have you ever had blood clots?")]
        [Required]
        public int? Question3 { get; set; }

        [Display(Name = "4. Are you sextually active")]
        [Required]
        public int? Question4 { get; set; }
        [Display(Name = "5. Do you want protection against STIs?")]
        [Required]
        public int? Question5 { get; set; }
        [Display(Name = "6. Have you ever Used any Contraceptives")]
        [Required]
        public int? Question6 { get; set; }
        [Display(Name = "7. When Last did you have your period")]
        [Required]
        public int? Question7 { get; set; }
        [Display(Name = "8. Are they Normal, Compared to the last two cycles?")]
        [Required]
        public int? Question8 { get; set; }
        [Display(Name = "9. Are you taking any medication?")]
        [Required]
        public int? Question9 { get; set; }
        [Display(Name = "10. Are you breast feeding?")]
        [Required]
        public int? Question10 { get; set; }
        public int Total { get; set; }
        public string? Message { get; set; }


        public FamilyPlanning_Screening()
        {
            Date = DateTime.Now;
            Status = "New";
        }

    }
}
