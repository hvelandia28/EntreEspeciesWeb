using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntreEspeciesNuevo.Models;
using Newtonsoft.Json;

namespace EntreEspeciesNuevo.Controllers
{
    public class ComprasController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public ComprasController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            bool RegistrarCompras = RegistrarCompra().Result;
            ViewBag.RegistrarCompras = RegistrarCompras;
            bool VerCompras = VerCompra().Result;
            ViewBag.VerCompras = VerCompras;

            return _context.Compras != null ?
                          View(await _context.Compras.ToListAsync()) :
                          Problem("Entity set 'EntreespeciessqlContext.Compras'  is null.");
        }
        public async Task<bool> RegistrarCompra()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 21)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }

        public async Task<bool> VerCompra()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 23)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .Include(c => c.DetaCompras)
                .FirstOrDefaultAsync(m => m.IdCompra == id);

            if (compra == null)
            {
                return NotFound();
            }

            // Cargar el nombre del producto para cada detalle
            foreach (var detalle in compra.DetaCompras)
            {
                detalle.ProductoNombre = await ObtenerNombreProducto(detalle.IdProducto);
            }

            return View(compra);
        }

        private async Task<string> ObtenerNombreProducto(int? idProducto)
        {
            if (idProducto == null)
            {
                return "Producto no disponible";
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.IdProducto == idProducto);

            return producto?.NomProducto ?? "Producto no disponible";
        }

        // GET: Compras/Create
        public IActionResult Create()
        {
            // Filtrar productos con disponibilidad
            var productosConDisponibilidad = _context.Productos
                .Where(p => p.Disponibilidad > 0)
                .ToList();
            // Obtener la lista de IDs de categorías asociadas a productos con disponibilidad
            var idsCategoriasConProductosDisponibles = _context.Productos
                .Where(p => p.Disponibilidad > 0)
                .Select(p => p.IdCategoria)
                .Distinct()
                .ToList();

            // Filtrar categorías basadas en los IDs obtenidos
            var categoriasFiltradas = _context.Categorias
                .Where(c => idsCategoriasConProductosDisponibles.Contains(c.IdCategoria))
                .ToList();

            // Crear el modelo de vista
            var viewModel = new CompraViewModel
            {
                Compras = new List<Compra> { new Compra { DetaCompras = new List<DetaCompra>() } }, // Inicializa la lista de compras y detalles
                DetaCompras = new List<DetaCompra> { new DetaCompra() } // Inicializa la lista de detalles de compra con un elemento
            };
            ViewData["IdCategoria"] = new SelectList(categoriasFiltradas, "IdCategoria", "NomCategoria");
            ViewBag.ProductosConDisponibilidad = new SelectList(productosConDisponibilidad, "IdProducto", "NomProducto");
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "NomProveedor");
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompraViewModel viewModel, string DetallesTemporales)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int? totalCompra = 0;  // Variable para almacenar el total de la compra
                    int? preciopro = 0;

                    // Agregar cada compra al contexto
                    foreach (var compra in viewModel.Compras)
                    {
                        // Establecer la fecha de compra como la fecha y hora actual
                        compra.FechaCompra = DateTime.Now;
                        _context.Add(compra);

                        // Guardar cambios para obtener el IdCompra generado automáticamente
                        await _context.SaveChangesAsync();

                        // Procesar detalles temporales
                        if (!string.IsNullOrEmpty(DetallesTemporales))
                        {
                            var detallesTemporalesDeserializados = JsonConvert.DeserializeObject<List<DetaCompra>>(DetallesTemporales);

                            foreach (var detalleTemporal in detallesTemporalesDeserializados)
                            {
                                detalleTemporal.IdCompra = compra.IdCompra;
                                _context.Add(detalleTemporal);
                                preciopro = detalleTemporal.PrecioCompra / detalleTemporal.Cantidad;
                                // Actualizar la cantidad en la tabla de productos
                                var producto = _context.Productos.Find(detalleTemporal.IdProducto);
                                if (producto != null)
                                {
                                    producto.Precio = preciopro;
                                    producto.Cantidad += detalleTemporal.Cantidad;
                                    _context.Update(producto);
                                }
                                // Sumar al total de la compra
                                totalCompra += detalleTemporal.PrecioCompra ?? 0;
                            }

                            await _context.SaveChangesAsync();
                        }
                    }

                    // Actualizar el total de la compra en la entidad de compra
                    foreach (var compra in viewModel.Compras)
                    {
                        compra.Total = totalCompra;
                        _context.Update(compra);
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index"); // Redireccionar a la página de índice de compras
                }
                catch (Exception ex)
                {
                    // Manejar excepciones si es necesario
                    // Puedes registrar el error o proporcionar alguna información al usuario
                    ModelState.AddModelError(string.Empty, "Error al procesar la compra. Por favor, inténtalo de nuevo.");
                }
            }

            // Si ModelState no es válido, regresa a la vista de creación con los errores de validación
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult ObtenerProductosPorCategoria(int idCategoria)
        {
            var productosPorCategoria = _context.Productos
                .Where(p => p.IdCategoria == idCategoria && p.Disponibilidad > 0)
                .Select(p => new { IdProducto = p.IdProducto, NomProducto = p.NomProducto })
                .ToList();

            return Json(productosPorCategoria);
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCompra,FechaCompra,FormaPago")] Compra compra)
        {
            if (id != compra.IdCompra)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.IdCompra))
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
            return View(compra);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Compras == null)
            {
                return NotFound();
            }

            var compra = await _context.Compras
                .FirstOrDefaultAsync(m => m.IdCompra == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Compras == null)
            {
                return Problem("Entity set 'EntreespeciessqlContext.Compras'  is null.");
            }
            var compra = await _context.Compras.FindAsync(id);
            if (compra != null)
            {
                _context.Compras.Remove(compra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
            return (_context.Compras?.Any(e => e.IdCompra == id)).GetValueOrDefault();
        }
    }
}
