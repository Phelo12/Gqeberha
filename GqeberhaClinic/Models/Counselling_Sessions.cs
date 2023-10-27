using GqeberhaClinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GqeberhaClinic.Models
{
    public class Counselling_Sessions
    {
        [Key]
        public int SessionID { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Session Created Date")]
        public DateTime SessionCreatedDate { get; set;}
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Session Date")]
        public DateTime SessionDate { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Session Time")]
        public DateTime SessionTime { get; set; }
        public string CounsellorID { get; set; }
        [ForeignKey("CounsellorID")]
        public virtual GqebheraUser Counsellor { get; set; }
        public int? AppointmentID { get; set; }
        [ForeignKey("AppointmentID")]
        public virtual Appointments Appointment { get; set; }

        public string? Notes { get; set; }
        public string Status { get; set; }
        public Counselling_Sessions()
        {
            SessionCreatedDate = DateTime.Now;
            Status = "New";
        }



    }
}
