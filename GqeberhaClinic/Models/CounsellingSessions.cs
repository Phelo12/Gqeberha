using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GqeberhaClinic.Models
{
    public class CounsellingSessions
    {
        [Key]
        public int SessionID { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Session Created Date")]
        public DateTime CreatedDate { get; set;}
        [Required]
        [DataType(DataType.Date)]


    }
}
