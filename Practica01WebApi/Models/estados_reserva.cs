using System.ComponentModel.DataAnnotations;

namespace Practica01.Models
{
    public class Estados_reserva
    {
        [Key]
        public int estado_res_id { get; set; }
        public string? estado { get; set; }

    }
}
