using System.ComponentModel.DataAnnotations;

namespace Exercise8.Models
{
    public class Medicament
    {
        public int IdMedicament { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;
        public virtual ICollection<PrescriptionMedicament> PerscriptionMedicaments { get; set; } = null!;
    }
}
