using GqeberhaClinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GqeberhaClinic.Models
{
    public class Records
    {
        [Key]
        public int RecordsID { get; set; }
        public int FileID { get; set; }
        [ForeignKey(nameof(FileID))]
        public virtual Medical_File? File { get; set; }
        [Required]
        [Display(Name = "Blood pressure")]
        public decimal? BloodPressure { get; set; }
        [Required]
        [Display(Name = "Heart rate")]
        public int? HeartRate { get; set; }
        [Required]
        [Display(Name = "Body Temparature (°C)")]
        public decimal? Temperature { get; set; }
        [Required]

        [Display(Name = "Height (Cm)")]
        public decimal? Height { get; set; }
        [Required]
        [Display(Name = "Weight (Kg)")]
        public decimal? Weight { get; set; }
        [Required]
        // Notes and Observations
        public string? Notes { get; set; }

        //Date OF Birth

        [Display(Name = "Record Date")]
        public DateTime RecordDate { get; set; } = DateTime.Now;


        public string? NurseID { get; set; }
        [ForeignKey(nameof(NurseID))]
        public virtual GqebheraUser? Nurse { get; set; }


    }
}
