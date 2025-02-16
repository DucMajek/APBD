using System.ComponentModel.DataAnnotations;

namespace Exercise8.Models
{
    public class Doctor
    {
        [Key]
        public int IdDoctor { get; set; }
        [Required]
        [MaxLength(120)]
        public string FirstName { get; set; } = null!;
        [Required]
        [MaxLength(120)]
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(120)]
        public string Email { get; set; } = null!;

        public virtual ICollection<Prescription> Prescriptions { get; set; } = null!;
    }
}
