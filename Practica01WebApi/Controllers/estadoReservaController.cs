using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estadoReservaController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public estadoReservaController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("getall")]

        public IActionResult getAll()
        {
            var reser = (from db in _equiposContext.estados_reserva
                         select db).ToList();

            if (reser.Count == 0) { return NotFound(); }

            return Ok(reser);
        }

        [HttpGet]
        [Route("getbyid/{id}")]

        public IActionResult get(int id)
        {
            var marc = (from db in _equiposContext.estados_reserva
                         where db.estado_res_id == id
                         select db).FirstOrDefault();

            if (marc == null)
            {
                return NotFound();
            }
            return Ok(marc);
        }

        [HttpGet]
        [Route("find")]

        public IActionResult find(string filtro)
        {
            var marc = (from db in _equiposContext.estados_reserva
                         where db.estado == filtro
                         select db).ToList();

            if (marc.Any())
            {
                return Ok(marc);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult post([FromBody] Estados_reserva marcNew)
        {
            try
            {

                _equiposContext.estados_reserva.Add(marcNew);
                _equiposContext.SaveChanges();
                return Ok(marcNew);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult update(int id, [FromBody] Estados_reserva modificar)
        {
            Estados_reserva? marc = (from e in _equiposContext.estados_reserva
                             where e.estado_res_id == id
                             select e).FirstOrDefault();

            if (marc == null)
            {
                return NotFound();
            }

            marc.estado = modificar.estado;


            _equiposContext.estados_reserva.Entry(marc).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(marc);

        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult eliminar(int id)
        {
            Estados_reserva? marc = (from e in _equiposContext.estados_reserva
                             where e.estado_res_id == id
                             select e).FirstOrDefault();
            if (marc == null) { return NotFound(); }
            _equiposContext.estados_reserva.Attach(marc);
            _equiposContext.estados_reserva.Remove(marc);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }
}
