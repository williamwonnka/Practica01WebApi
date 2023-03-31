using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica01.Models;

namespace Practica01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuarioController : ControllerBase
    {
        private readonly equiposContext _equiposContext;

        public usuarioController(equiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        [HttpGet]
        [Route("getall")]
        public IActionResult GetAll() 
        {
            var user = (from db in _equiposContext.usuarios
                        join car in _equiposContext.carreras
                            on db.carrera_id equals car.carrera_id
                        select new
                        {
                            db.usuario_id,
                            db.nombre,
                            db.documento,
                            db.carnet,
                            car.carrera_id,
                            car.nombre_carrera
                        }).ToList();

            if (user.Count == 0) { return NotFound(); }

            return Ok(user);

        }

        [HttpGet]
        [Route("getbyid/{id}")]
        public IActionResult getId (int id)
        {
            var user = (from db in _equiposContext.usuarios
                        join car in _equiposContext.carreras
                            on db.carrera_id equals car.carrera_id
                        where db.usuario_id== id
                        select new
                        {
                            db.usuario_id,
                            db.nombre,
                            db.documento,
                            db.carnet,
                            car.carrera_id,
                            car.nombre_carrera
                        }).FirstOrDefault();

            if (user == null) { return NotFound(); }

            return Ok(user);
        }

        [HttpGet]
        [Route("find")]
        public IActionResult find (string filtro) 
        {
            var user = (from db in _equiposContext.usuarios
                        join car in _equiposContext.carreras
                            on db.carrera_id equals car.carrera_id
                        where db.nombre == filtro || db.documento == filtro || db.carnet == filtro
                        select new
                        {
                            db.usuario_id,
                            db.nombre,
                            db.documento,
                            db.carnet,
                            car.carrera_id,
                            car.nombre_carrera
                        }).ToList();

            if (user.Any())
            {
                return Ok(user);
            }

            return NotFound();

        }

        [HttpPost]
        [Route("add")]
        public IActionResult add([FromBody] usuarios userN)
        {
            try
            {
                _equiposContext.usuarios.Add(userN);
                _equiposContext.SaveChanges();
                return Ok(userN);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult actualizarEquipo(int id, [FromBody] usuarios modificar)
        {
            usuarios? user = (from e in _equiposContext.usuarios
                               where e.usuario_id == id
                               select e).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            user.nombre = modificar.nombre;
            user.documento = modificar.documento;
            user.tipo = modificar.tipo;
            user.carrera_id = modificar.carrera_id;

            _equiposContext.usuarios.Entry(user).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(user);

        }

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public IActionResult eliminar(int id)
        {
            usuarios? user = (from e in _equiposContext.usuarios
                               where e.usuario_id == id
                               select e).FirstOrDefault();
            if (user == null) { return NotFound(); }
            _equiposContext.usuarios.Attach(user);
            _equiposContext.usuarios.Remove(user);
            _equiposContext.SaveChanges();

            return Ok();
        }
    }
}
