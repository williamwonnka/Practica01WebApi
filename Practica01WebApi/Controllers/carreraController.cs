using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class carreraController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public carreraController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll()
        {
            var carrera = (from db in _equiposContext.carreras
                           join fac in _equiposContext.facultades
                                on db.facultad_id equals fac.facultad_id
                            select new
                            {
                                db.carrera_id,
                                db.nombre_carrera,
                                fac.facultad_id,
                                fac.nombre_facultad
                            }).ToList();

            if (carrera.Count == 0) { return NotFound(); }

            return Ok(carrera);
        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public IActionResult GetId(int id) 
        {
            var marca = (from db in _equiposContext.carreras
                         join fac in _equiposContext.facultades
                              on db.facultad_id equals fac.facultad_id
                         where db.facultad_id == id
                         select new
                         {
                             db.carrera_id,
                             db.nombre_carrera,
                             fac.facultad_id,
                             fac.nombre_facultad
                         }).FirstOrDefault();

            if (marca == null)
            {
                return NotFound();
            }
            return Ok(marca);

        }

        [HttpGet]
        [Route("find")]
        public IActionResult find(string filtro)
        {
            var marca = (from db in _equiposContext.carreras
                         join fac in _equiposContext.facultades
                              on db.facultad_id equals fac.facultad_id
                         where db.nombre_carrera == filtro || fac.nombre_facultad==filtro
                         select new
                         {
                             db.carrera_id,
                             db.nombre_carrera,
                             fac.facultad_id,
                             fac.nombre_facultad
                         }).ToList();

            if (marca.Any())
            {
                return Ok(marca);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult crear([FromBody] carreras carreraNew)
        {
            try
            {
                _equiposContext.carreras.Add(carreraNew);
                _equiposContext.SaveChanges();
                return Ok(carreraNew);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult update(int id, [FromBody] carreras modificar)
        {
            carreras? carrera = (from e in _equiposContext.carreras
                               where e.carrera_id == id
                               select e).FirstOrDefault();

            if (carrera == null)
            {
                return NotFound();
            }

            carrera.nombre_carrera = modificar.nombre_carrera;

            _equiposContext.carreras.Entry(carrera).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(carrera);

        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult eliminar(int id)
        {
            carreras? carrera = (from e in _equiposContext.carreras
                               where e.carrera_id == id
                               select e).FirstOrDefault();
            if (carrera == null) { return NotFound(); }
            _equiposContext.carreras.Attach(carrera);
            _equiposContext.carreras.Remove(carrera);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }
}
