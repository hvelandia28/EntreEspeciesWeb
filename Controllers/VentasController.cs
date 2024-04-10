using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntreEspeciesNuevo.Models;
using X.PagedList;
using Newtonsoft.Json;

namespace EntreEspeciesNuevo.Controllers
{
    public class VentasController : Controller
    {
        private readonly EntreespeciessqlContext _context;

        public VentasController(EntreespeciessqlContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            bool RegistrarVentas = await RegistrarVenta();
            ViewBag.RegistrarVentas = RegistrarVentas;
            bool VerVentas = await VerVenta();
            ViewBag.VerVentas = VerVentas;

            var entreespeciessqlContext = _context.Ventas.Include(v => v.DocumentoClienteNavigation);
            return View(await entreespeciessqlContext.ToListAsync());
        }
        public async Task<bool> RegistrarVenta()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 17)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> VerVenta()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 18)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ventas == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.DocumentoClienteNavigation)
                .Include(c => c.DetaVenta)
                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (venta == null)
            {
                return NotFound();
            }
            // Cargar el nombre del producto para cada detalle
            foreach (var detalle in venta.DetaVenta)
            {
                detalle.ProductoNombre = await ObtenerNombreProducto(detalle.IdProducto);
                detalle.CitaNombre = await ObtenerNombreCita(detalle.IdCitaInterna);
            }

            return View(venta);
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
        private async Task<string> ObtenerNombreCita(int? idProducto)
        {
            if (idProducto == null)
            {
                return "Producto no disponible";
            }

            var producto = await _context.CitaInternas
                .FirstOrDefaultAsync(p => p.IdCitaInt == idProducto);

            return producto?.NomCita ?? "Producto no disponible";
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            // Filtrar productos con disponibilidad
            var productosConDisponibilidad = _context.Productos
                .Where(p => p.Disponibilidad > 0 && p.Cantidad>0)
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
            var viewModel = new VentaViewModel
            {
                Ventas = new List<Venta> { new Venta { DetaVenta = new List<DetaVentum>() } }, // Inicializa la lista de compras y detalles
                DetalleVentas = new List<DetaVentum> { new DetaVentum() } // Inicializa la lista de detalles de compra con un elemento
            };
            ViewData["IdCategoria"] = new SelectList(categoriasFiltradas, "IdCategoria", "NomCategoria");
            ViewBag.ProductosConDisponibilidad = new SelectList(productosConDisponibilidad, "IdProducto", "NomProducto");
            ViewBag.IdCitaInt = new SelectList(_context.CitaInternas, "IdCitaInt", "NomCita");
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente");
            return View(viewModel);
        }   

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VentaViewModel viewModel, string DetallesTemporales)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int? totalCompra = 0;  // Variable para almacenar el total de la compra

                    // Agregar cada compra al contexto
                    foreach (var compra in viewModel.Ventas)
                    {
                        compra.FechaVenta = DateTime.Now;
                        _context.Add(compra);

                        // Guardar cambios para obtener el IdCompra generado automáticamente
                        await _context.SaveChangesAsync();

                        // Procesar detalles temporales
                        if (!string.IsNullOrEmpty(DetallesTemporales))
                        {
                            var detallesTemporalesDeserializados = JsonConvert.DeserializeObject<List<DetaVentum>>(DetallesTemporales);

                            foreach (var detalleTemporal in detallesTemporalesDeserializados)
                            {
                                detalleTemporal.IdVenta = compra.IdVenta;
                                _context.Add(detalleTemporal);

                                // Actualizar la cantidad en la tabla de productos
                                var producto = _context.Productos.Find(detalleTemporal.IdProducto);
                                if (producto != null)
                                {
                                    producto.Cantidad -= detalleTemporal.Cantidad;
                                    _context.Update(producto);
                                }
                                if(detalleTemporal.IdCitaInterna != null)
                                {
                                    var citaInterna = await _context.CitaInternas.FindAsync(detalleTemporal.IdCitaInterna);
                                    citaInterna.Estado = "Realizada";
                                    _context.Update(citaInterna);

                                }

                                // Sumar al total de la compra
                                totalCompra += detalleTemporal.SubTotalSer ?? 0 + detalleTemporal.SubTotalPro ?? 0;
                            }

                            await _context.SaveChangesAsync();
                        }
                    }

                    // Actualizar el total de la compra en la entidad de compra
                    foreach (var compra in viewModel.Ventas)
                    {
                        compra.Total = totalCompra;
                        _context.Update(compra);
                    }

                    //// Actualizar el estado de las citas internas asociadas
                    //foreach (var detalleVenta in viewModel.DetalleVentas)
                    //{
                    //    if (detalleVenta.IdCitaInterna != null)
                    //    {
                            
                            
                    //            var citaInterna = await _context.CitaInternas.FindAsync(detalleVenta.IdCitaInterna);
                                
                                
                    //                citaInterna.Estado = "Realizada";
                    //                _context.Update(citaInterna);
                                
                            
                    //    }
                    //}

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
                .Where(p => p.IdCategoria == idCategoria && p.Disponibilidad > 0 && p.Cantidad > 0)
                .Select(p => new { IdProducto = p.IdProducto, NomProducto = p.NomProducto })
                .ToList();

            return Json(productosPorCategoria);
        }
        [HttpGet]
        public IActionResult ObtenerCitaPorCategoria(int idCategoria)
        {
            var productosPorCategoria = _context.CitaInternas
                .Where(p => p.DocumentoCliente == idCategoria && p.Estado == "Pendiente")
                .Select(p => new { IdProducto = p.IdCitaInt, NomProducto = p.NomCita })
                .ToList();

            return Json(productosPorCategoria);
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ventas == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", venta.DocumentoCliente);
            return View(venta);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVenta,DocumentoCliente,FechaVenta,FormaPago,Total")] Venta venta)
        {
            if (id != venta.IdVenta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.IdVenta))
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
            ViewData["DocumentoCliente"] = new SelectList(_context.Clientes, "DocumentoCliente", "DocumentoCliente", venta.DocumentoCliente);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ventas == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.DocumentoClienteNavigation)
                .FirstOrDefaultAsync(m => m.IdVenta == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ventas == null)
            {
                return Problem("Entity set 'EntreespeciessqlContext.Ventas'  is null.");
            }
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
            {
                _context.Ventas.Remove(venta);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
          return (_context.Ventas?.Any(e => e.IdVenta == id)).GetValueOrDefault();
        }
    }
}
