using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Api_Inmobiliaria.DTOs;
using Api_Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api_Inmobiliaria.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class Propietarios : ControllerBase
	{
		private readonly DataContext context;

		private readonly IConfiguration configuration;
		public Propietarios(DataContext context, IConfiguration configuration)

		{
			this.context = context;
			this.configuration = configuration;
		}
		//obtener todos los propietarios...
		public async Task<IActionResult> ObtenerTodos()
		{
			var propietarios = await context.Propietario.ToListAsync();
			return Ok(propietarios);
		}

		//obtener perfil propietario... 
		[Authorize]
		[HttpGet("perfil")]

		public async Task<IActionResult> ObtenerPerfil()
		{
			int id = int.Parse(User?.Identity?.Name ?? "0");

			if (id == 0)
			{
				return BadRequest("Error en obtener Id Propietario");
			}

			var propietario = await context.Propietario.FindAsync(id);

			return Ok(propietario);
		}
		[Authorize]
		[HttpPut("editar")]
		public async Task<IActionResult> EditarPerfil([FromBody] PropietarioEditarDTO dto)
		{
			try
			{
				int id = int.Parse(User.Identity?.Name ?? "0");
				var propietario = await context.Propietario.FindAsync(id);

				if (propietario == null)
					return NotFound("Propietario no encontrado");

				propietario.Nombre = dto.Nombre;
				propietario.Apellido = dto.Apellido;
				propietario.Dni = dto.Dni;
				propietario.Telefono = dto.Telefono;
				propietario.Email = dto.Email;

				context.Propietario.Update(propietario);
				await context.SaveChangesAsync();

				return Ok(new { mensaje = "Perfil actualizado correctamente", propietario });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[Authorize]
		[HttpPut("cambiar-clave")]
		public async Task<IActionResult> CambiarClave([FromBody] CambiarClaveDTO dto)
		{
			try
			{
				int id = int.Parse(User.Identity?.Name ?? "0");
				var propietario = await context.Propietario.FindAsync(id);

				if (propietario == null)
					return NotFound("Propietario no encontrado");

				// Verificoo la  clave actual
				string hashedActual = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: dto.ClaveActual,
					salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"] ?? ""),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));

				if (hashedActual != propietario.Clave)
					return BadRequest("La clave actual es incorrecta");

				// aca Hasheoooo la nueva clave
				string hashedNueva = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: dto.ClaveNueva,
					salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"] ?? ""),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));

				propietario.Clave = hashedNueva;
				context.Propietario.Update(propietario);
				await context.SaveChangesAsync();

				return Ok(new { mensaje = "Contraseña actualizada correctamente" });
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("hash")]
		public async Task<IActionResult> Hashear([FromQuery] string clave)
		{
			string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: clave,
					salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"] ?? ""),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));
			return Ok(hashed);
		}
		// POST api/<controller>/login
		[HttpPost("login")]
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO)
		{
			try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: loginDTO.Clave,
					salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"] ?? ""),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));
				var p = await context.Propietario.FirstOrDefaultAsync(x => x.Email == loginDTO.Usuario);
				if (p == null || p.Clave != hashed)
				{
					return BadRequest("Nombre de usuario o clave incorrecta");
				}
				else
				{
					var secreto = configuration["TokenAuthentication:SecretKey"];
					if (string.IsNullOrEmpty(secreto))
						throw new Exception("Falta configurar TokenAuthentication:Secret");
					var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(secreto));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, p.IdPropietario.ToString()),
						new Claim("FullName", p.Nombre + " " + p.Apellido),
						new Claim(ClaimTypes.Role, "Propietario"),
					};

					var token = new JwtSecurityToken(
						issuer: configuration["TokenAuthentication:Issuer"],
						audience: configuration["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(60),
						signingCredentials: credenciales
					);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}






	}

}