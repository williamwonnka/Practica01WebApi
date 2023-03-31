using System.ComponentModel.DataAnnotations;

namespace Practica01.Models
{
    public class Marcas
    {
        [Key]
        public int id_marcas { get; set; }
        public string? nombre_marca { get; set; }
        public string? estados { get; set; }

    }
}
