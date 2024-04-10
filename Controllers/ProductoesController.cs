using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntreEspeciesNuevo.Models;
using Microsoft.AspNetCore.Hosting;

namespace EntreEspeciesNuevo.Controllers
{
    public class ProductoesController : Controller


    {
        private readonly EntreespeciessqlContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductoesController(EntreespeciessqlContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Productoes
        public async Task<IActionResult> Index()
        {
            bool RegistrarProductos = RegistrarProducto().Result;
            ViewBag.RegistrarProductos = RegistrarProductos;
            bool ActualizarProductos = ActualizarProducto().Result;
            ViewBag.ActualizarProductos = ActualizarProductos;
            bool CambioEstadoProductos = CambioEstadoProducto().Result;
            ViewBag.CambioEstadoProductos = CambioEstadoProductos;
            bool VerProductos = VerProducto().Result;
            ViewBag.VerProductos = VerProductos;
            var entreespeciessqlContext = _context.Productos.Include(p => p.IdCategoriaNavigation);
            return View(await entreespeciessqlContext.ToListAsync());
        }
        public async Task<bool> RegistrarProducto()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 14)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> ActualizarProducto()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 15)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> CambioEstadoProducto()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 16)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }
        public async Task<bool> VerProducto()
        {
            var username = User.Identity.Name;
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nombre == username);
            if (usuario == null)
            {
                return false;
            }
            var configuracion = await _context.Configuracions
                .Where(c => c.IdRol == usuario.IdRol && c.IdPermiso == 56)
                .FirstOrDefaultAsync();
            return configuracion != null;
        }

        // GET: Productoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)

                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }




        // GET: Productoes/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "NomCategoria");
            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "NomProveedor");
            return View();
        }

        // POST: Productoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,IdProveedor,IdCategoria,NomProducto,Disponibilidad,Cantidad,FechaVen,Precio")] Producto? producto, IFormFile Foto)
        {
            if (ModelState.IsValid)
            {
                // Primera comprobación de ModelState.IsValid
                if (Foto != null && Foto.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Foto.CopyToAsync(memoryStream);
                        producto.Foto = memoryStream.ToArray();
                    }
                }

                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Segunda comprobación de ModelState.IsValid
            if (ModelState.IsValid)
            {
                // Lógica para procesar el formulario
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "IdCategoria", producto.IdCategoria);
            return View(producto);
        }


        [HttpGet]
        public IActionResult GetPrecioProducto(int productId)
        {
            try
            {
                var producto = _context.Productos.FirstOrDefault(p => p.IdProducto == productId);

                if (producto != null)
                {
                    return Json(producto.Precio);
                }

                return NotFound(); // O devuelve un valor predeterminado, según tus requisitos.
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tus necesidades (por ejemplo, registrarla)
                return StatusCode(500, "Error interno del servidor");
            }

        }



        [HttpGet]
        public async Task<IActionResult> VerificarNombreExistentehjsgfs(string nuevoNombre)
        {
            bool nombreExistente = await _context.Productos.AnyAsync(p => p.NomProducto == nuevoNombre);
            return Json(new { existe = nombreExistente });
        }

        [HttpPost]
        public IActionResult VerificarNombreExistente(string nuevoNombre)
        {
            // Verificar si ya existe un producto con el mismo nombre
            bool nombreExiste = _context.Productos.Any(p => p.NomProducto == nuevoNombre);

            return Json(new { existe = nombreExiste });
        }

        public IActionResult VerificarCantidadNoNegativa(int nuevaCantidad)
        {
            bool cantidadNegativa = nuevaCantidad < 0;
            return Json(new { cantidadNegativa });
        }

        public IActionResult GetCantidad(int productId)
        {
            try
            {
                var producto = _context.Productos.FirstOrDefault(p => p.IdProducto == productId);

                if (producto != null)
                {
                    return Json(producto.Cantidad);
                }

                return NotFound(); // O devuelve un valor predeterminado, según tus requisitos.
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tus necesidades (por ejemplo, registrarla)
                return StatusCode(500, "Error interno del servidor");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleAvailability(int idProducto, int disponibilidad)
        {
            var producto = await _context.Productos.FindAsync(idProducto);

            if (producto != null)
            {
                // Verifica si la cantidad es igual o menor a 0 y actualiza la disponibilidad en consecuencia
                if (producto.Cantidad < 0)
                {
                    producto.Disponibilidad = 0;
                }
                else
                {
                    // Invertir el valor de disponibilidad (0 a 1 o 1 a 0)
                    producto.Disponibilidad = (disponibilidad == 1) ? 0 : 1;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Productoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "NomCategoria", producto.IdCategoria);
            return View(producto);
        }

        [HttpGet]
        public IActionResult VerificarNombreExistente1(string nuevoNombre)
        {
            bool nombreExistente = _context.Productos.Any(p => p.NomProducto == nuevoNombre);
            return Json(new { nombreExistente });
        }



        // POST: Productoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Productoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,IdProveedor,IdCategoria,NomProducto,Disponibilidad,Cantidad,FechaVen,Precio,Foto")] Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el producto existente desde la base de datos
                    var existingProduct = await _context.Productos.FindAsync(id);

                    // Actualizar solo los campos del producto que se han proporcionado en el formulario de edición
                    existingProduct.IdCategoria = producto.IdCategoria;
                    existingProduct.NomProducto = producto.NomProducto;
                    existingProduct.Disponibilidad = producto.Disponibilidad;
                    existingProduct.Cantidad = producto.Cantidad;
                    existingProduct.FechaVen = producto.FechaVen;
                    existingProduct.Precio = producto.Precio;

                    // Actualizar la disponibilidad a 0 si la cantidad es 0
                    if (existingProduct.Cantidad <= 0)
                    {
                        existingProduct.Disponibilidad = 0;
                    }

                    // Guardar los cambios en la base de datos
                    _context.Update(existingProduct);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index)); // Redirigir al índice después de editar exitosamente
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }


            ViewData["IdCategoria"] = new SelectList(_context.Categorias, "IdCategoria", "IdCategoria", producto.IdCategoria);
            return View(producto);
        }

        public IActionResult VerificarNombreExistenteEditar(string nuevoNombre, int idProducto)
        {
            var productoExistente = _context.Productos.FirstOrDefault(p => p.NomProducto == nuevoNombre && p.IdProducto != idProducto);
            if (productoExistente != null)
            {
                return Json(new { nombreExistente = true });
            }
            return Json(new { nombreExistente = false });
        }

        [HttpPost]
        public async Task<IActionResult> CambiarImagen(int id, IFormFile nuevaImagen)
        {
            // Encuentra el producto por su ID
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            // Actualiza la imagen del producto si se ha enviado una nueva imagen
            if (nuevaImagen != null && nuevaImagen.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await nuevaImagen.CopyToAsync(memoryStream);
                    producto.Foto = memoryStream.ToArray();
                }
            }

            // Guarda los cambios en la base de datos
            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); // Redirige de vuelta a la vista Index después de cambiar la imagen
        }

        [HttpGet]
        public async Task<IActionResult> VerificarNombreExistenteEditar1(string nuevoNombre, int idProducto)
        {
            bool nombreExistente = await _context.Productos.AnyAsync(p => p.NomProducto == nuevoNombre && p.IdProducto != idProducto);
            return Json(new { nombreExistente });
        }



        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'EntreespeciessqlContext.Productos'  is null.");
            }
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return (_context.Productos?.Any(e => e.IdProducto == id)).GetValueOrDefault();
        }
    }


}
