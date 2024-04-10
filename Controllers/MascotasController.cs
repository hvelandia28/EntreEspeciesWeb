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
    public class MascotasController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public MascotasController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: Mascotas
        public async Task<IActionResult> Index()
        {
            bool RegistrarMascotas = RegistrarMascota().Result;
            ViewBag.RegistrarMascotas = RegistrarMascotas;
            bool ActualizarMascotas = ActualizarMascota().Result;
            ViewBag.ActualizarMascotas = ActualizarMascotas;
            bool VerMascotas = VerMascota().Result;
            ViewBag.VerMascotas = VerMascotas;

            var model = await _context.Mascotas.Include(m => m.DocumentoClienteNavigation)
                                               .ToListAsync();

            return View(model);
        }

        public async Task<bool> RegistrarMascota()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 10)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> ActualizarMascota()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 12)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> VerMascota()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 11)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }

        // GET: Mascotas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas
                .Include(m => m.DocumentoClienteNavigation)
                .FirstOrDefaultAsync(m => m.IdMascota == id);

            if (mascota == null)
            {
                return NotFound();
            }

            return Json(new
            {
                documentoCliente = mascota.DocumentoCliente,
                nombreMascota = mascota.NombreMascota,
                fechaNacimiento = mascota.FechaNacimiento,
                colorMascota = mascota.ColorMascota,
                especie = mascota.Especie,
                raza = mascota.Raza,
                genero = mascota.Genero,
                infMascota = mascota.InfMascota
            });
        }


        //// GET: Mascotas/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null || _context.Mascotas == null)
        //    {
        //        return NotFound();
        //    }

        //    var mascota = await _context.Mascotas
        //        .Include(m => m.DocumentoClienteNavigation)
        //        .FirstOrDefaultAsync(m => m.IdMascota == id);
        //    if (mascota == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(mascota);
        //}

        //// GET: Mascotas/Create
        //public IActionResult Create()
        //{
        //    ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente");
        //    return View();
        //}
        //// POST: Mascotas/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("IdMascota,DocumentoCliente,NombreMascota,FechaNacimiento,ColorMascota,Especie,Raza,Genero,InfMascota")] Mascota mascota)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Verificar si el documento del cliente está registrado
        //        var clienteExistente = await _context.Clientes.FindAsync(mascota.DocumentoCliente);
        //        if (clienteExistente == null)
        //        {
        //            // Si el cliente no está registrado, agregar un error al modelo
        //            ModelState.AddModelError("DocumentoCliente", "El documento del cliente no está registrado.");
        //            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", mascota.DocumentoCliente);
        //            return View(mascota);
        //        }

        //        // Si el cliente está registrado, guardar la mascota y redirigir a la página de índice
        //        _context.Add(mascota);
        //        await _context.SaveChangesAsync();

        //        // Establecer el mensaje de éxito en TempData
        //        TempData["SuccessMessage"] = "La mascota se ha registrado correctamente.";

        //        return RedirectToAction(nameof(Index));
        //    }

        //    ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", mascota.DocumentoCliente);
        //    return View(mascota);
        //}

        // GET: Mascotas/Create
        public IActionResult Create()
        {
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente");
            return View();
        }

        // POST: Mascotas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMascota,DocumentoCliente,NombreMascota,FechaNacimiento,ColorMascota,Especie,Raza,Genero,InfMascota")] Mascota mascota)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mascota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Mensaje de alerta
            TempData["AlertMessage"] = "No se pudo registrar la mascota.";
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", mascota.DocumentoCliente);
            return View(mascota);
        }

        // Agrega este método dentro de tu clase MascotasController
        [HttpPost]
        public async Task<JsonResult> ValidarDocumentoCliente(int documentoCliente)
        {
            // Busca en la base de datos si el documentoCliente existe
            var existe = await _context.Clientes
                .AnyAsync(c => c.DocumentoCliente == documentoCliente);

            // Devuelve un objeto JSON con una propiedad 'existe' que indica si el documento está registrado
            return Json(new { existe = existe });
        }


        // GET: Mascotas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Mascotas == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota == null)
            {
                return NotFound();
            }
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", mascota.DocumentoCliente);
            return View(mascota);
        }

        // POST: Mascotas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMascota,DocumentoCliente,NombreMascota,FechaNacimiento,ColorMascota,Especie,Raza,Genero,InfMascota")] Mascota mascota)
        {
            if (id != mascota.IdMascota)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mascota);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MascotaExists(mascota.IdMascota))
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
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", mascota.DocumentoCliente);
            return View(mascota);
        }

        // GET: Mascotas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Mascotas == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas
                .Include(m => m.DocumentoClienteNavigation)
                .FirstOrDefaultAsync(m => m.IdMascota == id);
            if (mascota == null)
            {
                return NotFound();
            }

            return View(mascota);
        }

        // POST: Mascotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Mascotas == null)
            {
                return Problem("Entity set 'EntreespeciessqlContext.Mascotas'  is null.");
            }
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota != null)
            {
                _context.Mascotas.Remove(mascota);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MascotaExists(int id)
        {
          return (_context.Mascotas?.Any(e => e.IdMascota == id)).GetValueOrDefault();
        }
    }
}
