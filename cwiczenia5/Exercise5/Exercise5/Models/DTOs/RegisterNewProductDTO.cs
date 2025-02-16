using System.ComponentModel.DataAnnotations;

namespace Exercise5.Models.DTOs
{
    public class RegisterNewProductDTO
    {
        [Required]
        public int IdProduct { get; set; }

        [Required]
        public int IdWarehouse { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Amount should be bigger than 0!")]
        public int Amount { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
