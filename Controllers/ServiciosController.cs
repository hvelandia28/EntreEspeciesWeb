using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntreEspeciesNuevo.Models;
using X.PagedList;

namespace EntreEspeciesNuevo.Controllers
{
    public class ServiciosController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public ServiciosController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: Servicios
        public async Task<IActionResult> Index(int? page)
        {
            bool RegistrarServicios = await RegistrarServicio();
            ViewBag.RegistrarServicios = RegistrarServicios;
            bool EditarServicios = await EditarServicio();
            ViewBag.EditarServicios = EditarServicios;
            bool VerServicios = await VerServicio();
            ViewBag.VerServicios = VerServicios;
            bool BuscarServicios = await BuscarServicio();
            ViewBag.BuscarServicios = BuscarServicios;
            int pageSize = 99999;
            int pageNumber = page ?? 1;



            var servicios = await _context.Servicios.AsNoTracking().ToListAsync();

            var model = servicios.ToPagedList(pageNumber, pageSize);

            return View(model);
        }
        public async Task<bool> RegistrarServicio()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 35)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> EditarServicio()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 36)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }

        public async Task<bool> VerServicio()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 57)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> BuscarServicio()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 39)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }


        // GET: Servicios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Servicios == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios
                .FirstOrDefaultAsync(m => m.IdServicio == id);
            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }

        // GET: Servicios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Servicios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Servicios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdServicio,NomServico,Categoria,Precio")] Servicio servicio)
        {
            if (ModelState.IsValid)
            {
                // Verificar si ya existe un servicio con el mismo nombre
                if (_context.Servicios.Any(s => s.NomServico == servicio.NomServico))
                {
                    ModelState.AddModelError("NomServico", "Ya existe un servicio con este nombre.");
                    return RedirectToAction(nameof(Index));
                }

                _context.Add(servicio);
                await _context.SaveChangesAsync();

                // Devolver una respuesta JSON de éxito
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores de validación, devolver una respuesta JSON con los errores
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ValidarServicio(string nombre)
        {
            // Verificar si ya existe un cliente con el mismo número de documento
            bool existe = _context.Servicios.Any(s => s.NomServico == nombre);

            return Json(new { existe = existe });
        }

        // GET: Servicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Servicios == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio == null)
            {
                return NotFound();
            }
            return View(servicio);
        }
        [HttpGet]
        public IActionResult GetPrecioProducto(int servicioId)
        {
            try
            {
                var servicio = _context.Servicios.FirstOrDefault(s => s.IdServicio == servicioId);

                if (servicio != null)
                {
                    return Json(servicio.Precio);
                }

                return NotFound(); // O devuelve un valor predeterminado, según tus requisitos.
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tus necesidades (por ejemplo, registrarla)
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // POST: Servicios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdServicio,NomServico,Categoria,Precio")] Servicio servicio)
        {
            if (id != servicio.IdServicio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servicio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicioExists(servicio.IdServicio))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(servicio);
        }

        // GET: Servicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Servicios == null)
            {
                return NotFound();
            }

            var servicio = await _context.Servicios
                .FirstOrDefaultAsync(m => m.IdServicio == id);
            if (servicio == null)
            {
                return NotFound();
            }

            return View(servicio);
        }

        // POST: Servicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Servicios == null)
            {
                return Problem("Entity set 'EntreespeciessqlContext.Servicios'  is null.");
            }
            var servicio = await _context.Servicios.FindAsync(id);
            if (servicio != null)
            {
                _context.Servicios.Remove(servicio);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServicioExists(int id)
        {
          return (_context.Servicios?.Any(e => e.IdServicio == id)).GetValueOrDefault();
        }
    }
}
