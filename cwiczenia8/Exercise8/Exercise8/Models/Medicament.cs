using System.ComponentModel.DataAnnotations;

namespace Exercise8.Models
{
    public class Medicament
    {
        [Key]
        public int IdMedicament { get; set; }

        [Required]
        [MaxLength(120)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(120)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(120)]
        public string Type { get; set; } = null!;
        public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = null!;
    }
}
