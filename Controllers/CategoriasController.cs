using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntreEspeciesNuevo.Models;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;


namespace EntreEspeciesNuevo.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public CategoriasController(EntreespeciessqlContext context)
        {
            _context = context;
        }
        private bool CategoriaTieneProductos(int idCategoria)
        {
            // Verificar si hay productos asociados a la categoría
            bool tieneProductos = _context.Productos.Any(p => p.IdCategoria == idCategoria);
            return tieneProductos;
        }

        // GET: Categorias
        public async Task<IActionResult> Index(int? page)
        {
            bool RegistrarCategorias = RegistrarCategoria().Result;
            ViewBag.RegistrarCategorias = RegistrarCategorias;
            bool ActualizarCategorias = ActualizarCategoria().Result;
            ViewBag.ActualizarCategorias = ActualizarCategorias;
            bool CambioEstadoCategorias = CambioEstadoCategoria().Result;
            ViewBag.CambioEstadoCategorias = CambioEstadoCategorias;
            bool VerCategorias = VerCategoria().Result;
            ViewBag.VerCategorias = VerCategorias;
            int pageSize = 9999999;
            int pageNumber = page ?? 1;

            var categorias = await _context.Categorias.AsNoTracking().ToListAsync();

            // Agregar información sobre si una categoría tiene productos o no al ViewBag
            ViewBag.CategoriasConProductos = categorias.ToDictionary(c => c.IdCategoria, c => CategoriaTieneProductos(c.IdCategoria));

            var model = categorias.ToPagedList(pageNumber, pageSize);
            return View(model);
        }
        public async Task<bool> RegistrarCategoria()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 53)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> ActualizarCategoria()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 54)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> CambioEstadoCategoria()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 55)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> VerCategoria()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 52)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }


        // GET: Categorias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.IdCategoria == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categorias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categorias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCategoria, NomCategoria, Estado")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                // Verificar si la categoría ya existe
                bool categoriaExistente = _context.Categorias.Any(c => c.NomCategoria == categoria.NomCategoria);

                if (categoriaExistente)
                {
                    ModelState.AddModelError("NomCategoria", "La categoría ya existe.");
                    return RedirectToAction(nameof(Index));
                }

                // Si no existe, agregarla a la base de datos
                _context.Add(categoria);
                await _context.SaveChangesAsync();

                // Usar TempData para almacenar mensaje de éxito
                TempData["SuccessMessage"] = "La categoría se ha registrado correctamente.";

                // Devolver una respuesta JSON indicando éxito
                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, return a JSON response indicating error
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ValidarCategoria(string nombre)
        {
            // Verificar si ya existe un cliente con el mismo número de documento
            bool existe = _context.Categorias.Any(c => c.NomCategoria == nombre);

            return Json(new { existe = existe });
        }
        public JsonResult ValidarCategoriaEditar(string nombre, int id)
        {
            bool existe = _context.Categorias.Any(c => c.NomCategoria == nombre && c.IdCategoria != id);
            return Json(new { existe = existe });
        }


        [HttpPost]
        public async Task<IActionResult> CambiarEstadoCategoria(int id, string nuevoEstado)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            categoria.Estado = nuevoEstado;
            _context.Update(categoria);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Estado actualizado exitosamente" });
        }

        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // POST: Categorias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCategoria,NomCategoria")] Categoria categoria)
        {
            if (id != categoria.IdCategoria)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (EsCategoriaVacia(categoria))
                {
                    ModelState.AddModelError("", "Por favor, Debes escribir un nombre antes de guardar.");
                    return View(categoria);
                }

                try
                {
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.IdCategoria))
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

            return View(categoria);
        }

        private bool EsCategoriaVacia(Categoria categoria)
        {
            // Puedes personalizar la lógica según tus necesidades.
            return string.IsNullOrWhiteSpace(categoria.NomCategoria);
        }

        // GET: Categorias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categorias == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(m => m.IdCategoria == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // POST: Categorias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categorias == null)
            {
                return Problem("Entity set 'EntreespeciessqlContext.Categorias'  is null.");
            }
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
          return (_context.Categorias?.Any(e => e.IdCategoria == id)).GetValueOrDefault();
        }
    }
}
