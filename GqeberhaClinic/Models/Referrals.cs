using GqeberhaClinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GqeberhaClinic.Models
{
    public class Referrals
    {
        [Key]
        public int ReferralId { get; set; }
        public string? NurseID { get; set; }
        public GqebheraUser? Nurse { get; set; }
        public DateTime ReferralDate { get; set; } = DateTime.Now;
        public string? ReasonForReferral { get; set; }
        public string? ReferredTo { get; set; } // Could be the name of a specialist, clinic, or hospital
        public string? Notes { get; set; } // Additional details or observations
        public int? PreventionID { get; set; }
        [ForeignKey(nameof(PreventionID))]
        public ContraceptivePrescription? ContraceptivePlan { get; set; }
    }
}
