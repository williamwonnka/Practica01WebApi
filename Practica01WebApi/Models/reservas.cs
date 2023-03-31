using System.ComponentModel.DataAnnotations;

namespace Practica01.Models
{
    public class reservas
    {
        [Key]
        public int reserva_id { get; set; }
        [Required]
        public int equipo_id { get; set; }
        [Required]
        public int usuario_id { get; set; }
        public DateTime fecha_salida { get; set; }
        public DateTime hora_salida { get; set; }
        public int tiempo_reserva { get; set; }
        [Required]
        public int estado_reserva_id { get; set; }
        public DateTime fecha_retorno { get; set; }
        public DateTime hora_retorno { get; set; }

    }
}
