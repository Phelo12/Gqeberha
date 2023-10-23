using System.ComponentModel.DataAnnotations;

namespace GqeberhaClinic.Models
{
    public class Contraceptive
    {
        [Key]
        public int Id { get; set; }  // Unique Identifier
        public string? Name { get; set; }
        [MaxLength(5000)]
        public string? Type { get; set; }
        [MaxLength(5000)]
        public string? DurationOfEffectiveness { get; set; }
        [MaxLength(5000)]
        public string? MethodOfUse { get; set; }
        [MaxLength(5000)]
        public string? EfficacyRate { get; set; }
        [MaxLength(5000)]
        public string? SideEffects { get; set; }
        [MaxLength(5000)]
        public string? Benefits { get; set; }
        [MaxLength(5000)]
        public string? Cost { get; set; }
        [MaxLength(5000)]
        public string? Contraindications { get; set; }
        [MaxLength(5000)]
        public string? Reversibility { get; set; }
        [MaxLength(5000)]
        public string? StorageInstructions { get; set; }
        [MaxLength(5000)]
        public string? AdditionalNotes { get; set; }
    }
}
