using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoEquipoController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public tipoEquipoController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult getAll()
        {
            var tipo_Equipos = (from db in _equiposContext.tipo_equipo
                         select db).ToList();

            if (tipo_Equipos.Count == 0) { return NotFound(); }

            return Ok(tipo_Equipos);
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public IActionResult get(int id)
        {
            var tipo_Equipo = (from db in _equiposContext.tipo_equipo
                         where db.id_tipo_equipo == id
                         select db).FirstOrDefault();

            if (tipo_Equipo == null)
            {
                return NotFound();
            }
            return Ok(tipo_Equipo);
        }

        [HttpGet]
        [Route("find")]
        public IActionResult find(string filtro)
        {
            var tipo_Equipos = (from db in _equiposContext.tipo_equipo
                         where db.descripcion == filtro
                         select db).ToList();

            if (tipo_Equipos.Any())
            {
                return Ok(tipo_Equipos);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult post([FromBody] Tipo_equipo tipoNew)
        {
            try
            {

                _equiposContext.tipo_equipo.Add(tipoNew);
                _equiposContext.SaveChanges();
                return Ok(tipoNew);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult update(int id, [FromBody] Tipo_equipo modificar)
        {
            Tipo_equipo? tipo = (from e in _equiposContext.tipo_equipo
                             where e.id_tipo_equipo == id
                             select e).FirstOrDefault();

            if (tipo == null)
            {
                return NotFound();
            }

            tipo.descripcion = modificar.descripcion;


            _equiposContext.tipo_equipo.Entry(tipo).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(tipo);

        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult eliminar(int id)
        {
            Tipo_equipo? tipo_Equipo = (from e in _equiposContext.tipo_equipo
                             where e.id_tipo_equipo == id
                             select e).FirstOrDefault();
            if (tipo_Equipo == null) { return NotFound(); }
            _equiposContext.tipo_equipo.Attach(tipo_Equipo);
            _equiposContext.tipo_equipo.Remove(tipo_Equipo);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }
}
