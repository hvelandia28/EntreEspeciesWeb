using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EntreEspeciesNuevo.Models;

namespace EntreEspeciesNuevo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaApiController : ControllerBase
    {
        private readonly EntreespeciessqlContext _context;

        public CategoriaApiController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: api/CategoriaApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }

        // GET: api/CategoriaApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }
    }
}