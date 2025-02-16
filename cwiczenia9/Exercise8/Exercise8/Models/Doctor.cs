using System.ComponentModel.DataAnnotations;

namespace Exercise8.Models
{
    public class Doctor
    {
        public int IdDoctor { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public virtual ICollection<Prescription> Prescriptions { get; set; } = null!;
    }
}
