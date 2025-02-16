using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exercise8.Models
{
    [PrimaryKey(nameof(IdMedicament), nameof(IdPrescription))]
    public class PrescriptionMedicament
    {

        [ForeignKey(nameof(IdMedicament))]
        public int IdMedicament { get; set; }

        [ForeignKey(nameof(IdPrescription))]
        public int IdPrescription { get; set; }
        public int Dose { get; set; }
        public string Details { get; set; } = null!;

        public virtual Medicament Medicament { get; set; } = null!;
        public virtual Prescription Prescription { get; set; } = null!;
    }
}
