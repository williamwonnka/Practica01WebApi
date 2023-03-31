using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estadoEquipoController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public estadoEquipoController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult getAll()
        {
            var esta_Equipos = (from db in _equiposContext.estados_equipo
                                select db).ToList();

            if (esta_Equipos.Count == 0) { return NotFound(); }

            return Ok(esta_Equipos);
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public IActionResult get(int id)
        {
            var esta_Equipo = (from db in _equiposContext.estados_equipo
                               where db.id_estados_equipo == id
                               select db).FirstOrDefault();

            if (esta_Equipo == null)
            {
                return NotFound();
            }
            return Ok(esta_Equipo);
        }

        [HttpGet]
        [Route("find")]
        public IActionResult find(string filtro)
        {
            var esta_Equipos = (from db in _equiposContext.estados_equipo
                                where db.descripcion == filtro
                                select db).ToList();

            if (esta_Equipos.Any())
            {
                return Ok(esta_Equipos);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult post([FromBody] Estados_equipo estaNew)
        {
            try
            {

                _equiposContext.estados_equipo.Add(estaNew);
                _equiposContext.SaveChanges();
                return Ok(estaNew);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult update(int id, [FromBody] Estados_equipo modificar)
        {
            Estados_equipo? tipo = (from e in _equiposContext.estados_equipo
                                 where e.id_estados_equipo == id
                                 select e).FirstOrDefault();

            if (tipo == null)
            {
                return NotFound();
            }

            tipo.descripcion = modificar.descripcion;


            _equiposContext.estados_equipo.Entry(tipo).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(tipo);

        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult eliminar(int id)
        {
            Estados_equipo? esta_Equipo = (from e in _equiposContext.estados_equipo
                                        where e.id_estados_equipo == id
                                        select e).FirstOrDefault();
            if (esta_Equipo == null) { return NotFound(); }
            _equiposContext.estados_equipo.Attach(esta_Equipo);
            _equiposContext.estados_equipo.Remove(esta_Equipo);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }
}
