
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api_Inmobiliaria.Models;

namespace Api_Inmobiliaria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InmueblesController : ControllerBase
    {
        private readonly DataContext context;

        public InmueblesController(DataContext context)
        {
            this.context = context;
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerInmueblePorId(int id)
        {
            try
            {
                int idPropietario = int.Parse(User.Identity?.Name ?? "0");

                if (idPropietario == 0)
                    return BadRequest("No se pudo obtener el ID del propietario.");

                var inmueble = await context.Inmueble
                    .Where(i => i.IdInmueble == id && i.PropietarioId == idPropietario)
                    .Select(i => new
                    {
                        i.IdInmueble,
                        i.Direccion,
                        i.Tipo,
                        i.Ambientes,
                        i.Precio,
                        i.Habilitado,
                        i.Imagen
                    })
                    .FirstOrDefaultAsync();

                if (inmueble == null)
                    return NotFound("Inmueble no encontrado o no pertenece al propietario autenticado.");

                return Ok(inmueble);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al obtener el inmueble", error = ex.Message });
            }
        }

        // Obtener todos los inmueble del propietario logueado ..(Revisar )
        [Authorize]
        [HttpGet("propietario")]
        public async Task<IActionResult> ObtenerPorPropietario()
        {
            try
            {
                int idPropietario = int.Parse(User.Identity?.Name ?? "0");

                if (idPropietario == 0)
                    return BadRequest("No se pudo obtener el ID del propietario.");

                var inmuebles = await context.Inmueble

                .Where(i => i.PropietarioId == idPropietario)
                .Select(i => new
                {
                    i.IdInmueble,
                    i.Direccion,
                    i.Ambientes,
                    i.Precio,
                    i.Habilitado,
                    i.Imagen
                })

                .ToListAsync();

                return Ok(inmuebles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Endpoint para hablitar o deshabilitar

        [Authorize]
        [HttpPut("cambiarestado/{id}")]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            try
            {
                int idPropietario = int.Parse(User.Identity?.Name ?? "0");

                var inmueble = await context.Inmueble
                    .FirstOrDefaultAsync(i => i.IdInmueble == id && i.PropietarioId == idPropietario);

                if (inmueble == null)
                    return NotFound("Inmueble no encontrado o no pertenece al propietario.");

                inmueble.Habilitado = !inmueble.Habilitado;

                context.Inmueble.Update(inmueble);
                await context.SaveChangesAsync();

                string estado = inmueble.Habilitado ? "habilitado" : "deshabilitado";

                return Ok(new { mensaje = $"El inmueble fue {estado} correctamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // endpoint para agregar inmuebleee...
        [Authorize]
        [HttpPost("agregar")]
        public async Task<IActionResult> AgregarInmueble([FromForm] Inmueble nuevo, IFormFile? imagen)
        {
            try
            {
                int idPropietario = int.Parse(User.Identity?.Name ?? "0");

                nuevo.PropietarioId = idPropietario;
                nuevo.Habilitado = false; // deshabilitado por defecto

                if (imagen != null && imagen.Length > 0)
                {
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", "Inmuebles");
                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imagen.FileName)}";
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }

                    // Guardar ruta relativa
                    nuevo.Imagen = $"/Uploads/Inmuebles/{fileName}";
                }

                context.Inmueble.Add(nuevo);
                await context.SaveChangesAsync();

                return Ok(new { mensaje = "Inmueble agregado correctamente (pendiente de habilitar)", nuevo });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}