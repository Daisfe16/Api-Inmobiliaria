using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api_Inmobiliaria.Models;

namespace Api_Inmobiliaria.Controllers
{
    [ApiController] 
    [Authorize]
    [Route("api/[controller]")]
    public class ContratoController : ControllerBase
    {
        private readonly DataContext context;

        public ContratoController(DataContext context)
        {
            this.context = context;
        }


        // Listo loscontratos por inmuebles del propietario y sus pagos
        
        [HttpGet("inmuebles/{idInmueble}")]
        public async Task<IActionResult> ListarContratosPorInmueble(int idInmueble)
        {
            try
            {
                int idPropietario = int.Parse(User.Identity?.Name ?? "0");

                if (idPropietario == 0)
                    return BadRequest("No se pudo obtener el ID del propietario.");

                var inmueble = await context.Inmueble
                    .FirstOrDefaultAsync(i => i.IdInmueble == idInmueble && i.PropietarioId == idPropietario);

                if (inmueble == null)
                    return Unauthorized("El inmueble no pertenece al propietario autenticado.");

                var contratos = await context.Contrato
                    .Include(c => c.Inquilino)
                    .Include(c => c.Inmueble)
                    .Where(c => c.Inmueble != null && c.Inmueble.IdInmueble == idInmueble)
                    .Select(c => new
                    {
                        c.IdContrato,
                        c.FechaInicio,
                        c.FechaFin,
                        c.Monto,
                        c.Estado,
                        Inquilino = new
                        {
                            c.Inquilino.IdInquilino,
                            c.Inquilino.Nombre,
                            c.Inquilino.Apellido,
                            c.Inquilino.Email
                        },
                        Pagos = context.Pago
                            .Where(p => p.IdContrato == c.IdContrato)
                            .Select(p => new
                            {
                                p.IdPago,
                                p.NroPago,
                                p.Fecha,
                                p.Importe,
                                p.Detalle,
                                p.Estado
                            })
                            .OrderBy(p => p.NroPago)
                            .ToList()
                    })
                    .ToListAsync();

                if (contratos.Count == 0)
                    return NotFound("No se encontraron contratos para este inmueble.");

                return Ok(contratos);
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al obtener los contratos", error = ex.Message });
            }
        }

    }
}
