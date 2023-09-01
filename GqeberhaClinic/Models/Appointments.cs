using GqeberhaClinic.Areas.Identity.Data;
using Microsoft.Build.Execution;
using Microsoft.CodeAnalysis.Options;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace GqeberhaClinic.Models
{
    public class Appointments
    {
        [Key]
        public int AppointmentID { get; set; }
        [DataType(DataType.DateTime)]   
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Reason for making this appointment should be specified")]
        public string? Reason { get; set; }
        public string? Status { get; set; } = "New";
        public String? PatientID { get; set; }
        [ForeignKey("PatientID")]
        public virtual GqebheraUser? Patient { get; set; }  
        [Required]
        [Display(Name = "Date and Time")]
        [DataType(DataType.DateTime)]
        public DateTime Date_Time { get; set; } 
    }
}
