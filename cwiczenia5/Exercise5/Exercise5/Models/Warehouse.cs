using System.ComponentModel.DataAnnotations;

namespace Exercise5.Models
{
    public class Warehouse
    {
        public int idProduct { get; set; }
        public int idWarehouse { get; set; }
        public int amount { get; set; }
        public string createdAt { get; set; } = string.Empty;


    }
}
