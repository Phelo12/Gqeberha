using GqeberhaClinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GqeberhaClinic.Models
{
    public class MedicalRefill
    {
        [Key]
        public int Id { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public DateTime? ApprovalDate { get; set; } // Date when refill was approved
   
        public int QuantityRequested { get; set; }
        public RefillStatus Status { get; set; } = RefillStatus.Pending;
        public string? Notes { get; set; }
        public int PrescriptionId { get; set; }
        [ForeignKey(nameof(PrescriptionId))]
        public virtual Prescription? Prescription { get; set; }

        // Audit fields
        public string? PatientID { get; set; } // ID of the user (patient) requesting the refill
        [ForeignKey(nameof(PatientID))]
        public virtual GqebheraUser? Patient { get; set; }
        // Approval details
        public string? DoctorsID { get; set; }
        [ForeignKey(nameof(DoctorsID))]
        public virtual GqebheraUser? Doctor { get; set; }
    }
    public enum RefillStatus
    {
        Pending,
        Approved,
        Decliend,
        Completed,
        Packaging,
        Ready_For_Collection
    }


}
