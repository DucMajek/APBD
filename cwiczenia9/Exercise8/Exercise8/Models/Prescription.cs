using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Exercise8.Models
{
    
    public class Prescription
    {

        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public int IdDoctor { get; set; }
        public int IdPatient { get; set; }
        public virtual Patient Patients { get; set; } = null!;
        public virtual Doctor Doctors { get; set; } = null!;

        public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = null!;
    }
}
