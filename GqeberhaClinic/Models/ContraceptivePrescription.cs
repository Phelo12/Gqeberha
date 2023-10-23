using GqeberhaClinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GqeberhaClinic.Models
{
    public class ContraceptivePrescription
    {
        [Key]
        public int PrescriptionId { get; set; }
        public string? PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public GqebheraUser? Patient { get; set; }
        [Required]
        public string? ContraceptiveName { get; set; }
        [DataType(DataType.Date)]
    
        public DateTime PrescribedDate { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        [Required]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]

        public DateTime EndDate { get; set; }
        public string? Dosage { get; set; }

        public string? Frequency { get; set; }

        public string? Notes { get; set; }
        public string? Status { get; set; } = "Currently In Use";

    }
}
