using GqeberhaClinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GqeberhaClinic.Models
{
    public class Medical_Feedback
    {
        [Key]
        public int FeedbackID { get; set; }
        [Required]
        public int? PrescresptionID { get; set; }
        [ForeignKey(nameof(PrescresptionID))]
        public virtual Prescription? Prescription { get; set; }
        [Required]
        public string? Feedback { get; set; }
        public string? DoctorsFeedback { get; set; }
        public DateTime FeedbackDate { get; set; } = DateTime.Now;
        public DateTime? AnsweredDate { get; set; }
        public string? PatientID { get; set; } // ID of the user (patient) requesting the refill
        [ForeignKey(nameof(PatientID))]
        public virtual GqebheraUser? Patient { get; set; }
        // Approval details
        public string? DoctorsID { get; set; }
        [ForeignKey(nameof(DoctorsID))]
        public virtual GqebheraUser? Doctor { get; set; }



    }
}
