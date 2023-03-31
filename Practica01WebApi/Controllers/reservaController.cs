using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;
using System.Resources;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservaController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public reservaController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult getAll()
        {
            var reserva = (from db in _equiposContext.reservas
                           join eq in _equiposContext.equipos
                                on db.equipo_id equals eq.id_equipos
                           join us in _equiposContext.usuarios
                                on db.usuario_id equals us.usuario_id
                           join rese in _equiposContext.estados_reserva
                                on db.estado_reserva_id equals rese.estado_res_id
                            select new
                            {
                                db.reserva_id,
                                db.equipo_id,
                                eq.descripcion,
                                db.usuario_id,
                                us.nombre,
                                db.fecha_salida,
                                db.hora_salida,
                                db.tiempo_reserva,
                                db.estado_reserva_id,
                                rese.estado,
                                db.fecha_retorno,
                                db.hora_retorno

                            }).ToList();

            if (reserva.Count == 0) { return NotFound(); }

            return Ok(reserva);
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public IActionResult getId(int id)
        {
            var reserva = (from db in _equiposContext.reservas
                           join eq in _equiposContext.equipos
                                on db.equipo_id equals eq.id_equipos
                           join us in _equiposContext.usuarios
                                on db.usuario_id equals us.usuario_id
                           join rese in _equiposContext.estados_reserva
                                on db.estado_reserva_id equals rese.estado_res_id
                           where db.reserva_id == id
                           select new
                           {
                               db.reserva_id,
                               db.equipo_id,
                               eq.descripcion,
                               db.usuario_id,
                               us.nombre,
                               db.fecha_salida,
                               db.hora_salida,
                               db.tiempo_reserva,
                               db.estado_reserva_id,
                               rese.estado,
                               db.fecha_retorno,
                               db.hora_retorno

                           }).FirstOrDefault();

            if (reserva == null)
            {
                return NotFound();
            }
            return Ok(reserva);
        }

        [HttpGet]
        [Route("find")]
        public IActionResult find(string filtro)
        {
            var reserva = (from db in _equiposContext.reservas
                           join eq in _equiposContext.equipos
                                on db.equipo_id equals eq.id_equipos
                           join us in _equiposContext.usuarios
                                on db.usuario_id equals us.usuario_id
                           join rese in _equiposContext.estados_reserva
                                on db.estado_reserva_id equals rese.estado_res_id
                           where db.equipo_id == Int32.Parse(filtro) || db.usuario_id == Int32.Parse(filtro) ||
                                 db.fecha_salida == DateTime.Parse(filtro) || db.tiempo_reserva == Int32.Parse(filtro) ||
                                 db.fecha_retorno == DateTime.Parse(filtro)
                           select new
                           {
                               db.reserva_id,
                               db.equipo_id,
                               eq.descripcion,
                               db.usuario_id,
                               us.nombre,
                               db.fecha_salida,
                               db.hora_salida,
                               db.tiempo_reserva,
                               db.estado_reserva_id,
                               rese.estado,
                               db.fecha_retorno,
                               db.hora_retorno

                           }).ToList();

            if (reserva.Any())
            {
                return Ok(reserva);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult crear([FromBody] reservas reserN)
        {
            try
            {
                _equiposContext.reservas.Add(reserN);
                _equiposContext.SaveChanges();
                return Ok(reserN);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult actualizarEquipo(int id, [FromBody] reservas modificar)
        {
            reservas? reser = (from e in _equiposContext.reservas
                               where e.reserva_id == id
                               select e).FirstOrDefault();

            if (reser == null)
            {
                return NotFound();
            }

            reser.fecha_salida = modificar.fecha_salida;
            reser.hora_salida = modificar.hora_salida;
            reser.tiempo_reserva = modificar.tiempo_reserva;
            reser.fecha_retorno = modificar.fecha_retorno;
            reser.hora_retorno = modificar.hora_retorno;

            _equiposContext.reservas.Entry(reser).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(reser);

        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult eliminar(int id)
        {
            reservas? reser = (from e in _equiposContext.reservas
                               where e.reserva_id == id
                               select e).FirstOrDefault();
            if (reser == null) { return NotFound(); }
            _equiposContext.reservas.Attach(reser);
            _equiposContext.reservas.Remove(reser);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }
}
